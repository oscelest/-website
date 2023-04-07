using api.noxy.io.Context;
using api.noxy.io.Exceptions;
using api.noxy.io.Models.Game.Guild;
using api.noxy.io.Utilities;
using Microsoft.EntityFrameworkCore;
using System.Data;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using api.noxy.io.Models.Auth;
using System;
using Org.BouncyCastle.Asn1.X509;
using api.noxy.io.Utility;

namespace api.noxy.io.Interface
{
    public interface IGameRepository
    {
        public Task<GuildEntity?> LoadGuild(Guid userID);
        public Task<GuildEntity> CreateGuild(UserEntity user, string name);
        public Task<List<UnitEntity>> LoadUnitList(Guid userID);
        public Task<List<UnitEntity>> LoadUnitList(GuildEntity guild);
        public Task<List<MissionEntity>> LoadMissionList(Guid userID);
        public Task<List<MissionEntity>> LoadMissionList(GuildEntity guild);
        public Task<UnitEntity> InitiateUnit(Guid userID, Guid unitID);
        public Task<MissionEntity> InitiateMission(Guid userID, Guid missionID, List<Guid> listUnitID);
        public Task<List<UnitEntity>> RefreshAvailableUnitList(Guid userID);
        public Task<List<MissionEntity>> RefreshAvailableMissionList(Guid userID);
    }

    public class GameRepository : IGameRepository
    {
        private readonly APIContext _db;
        private readonly IApplicationConfiguration _config;

        public GameRepository(APIContext db, IApplicationConfiguration config)
        {
            _db = db;
            _config = config;
        }

        public async Task<GuildEntity?> LoadGuild(Guid userID)
        {
            return await _db.Guild.FirstOrDefaultAsync(x => x.User.ID == userID);
        }

        public async Task<List<UnitEntity>> LoadUnitList(Guid userID)
        {
            return await LoadUnitList(await GetGuild(userID));
        }

        public async Task<List<UnitEntity>> LoadUnitList(GuildEntity guild)
        {
            return await _db.Unit
                 .Include(x => x.UnitType).ThenInclude(x => x.EquipmentSlotList).ThenInclude(x => x.)
                 .Include(x => x.UnitRoleList).ThenInclude(x => x.Role).ThenInclude(x => x.RoleType)
                 .Where(x => x.Guild.ID == guild.ID)
                 .ToListAsync();
        }

        public async Task<List<MissionEntity>> LoadMissionList(Guid userID)
        {
            return await LoadMissionList(await GetGuild(userID));
        }

        public async Task<List<MissionEntity>> LoadMissionList(GuildEntity guild)
        {
            return await _db.Mission
                .Include(x => x.UnitList).ThenInclude(x => x.UnitRoleList).ThenInclude(x => x.Role).ThenInclude(x => x.RoleType)
                .Where(x => x.Guild.ID == guild.ID)
                .ToListAsync();
        }

        public async Task<GuildEntity> CreateGuild(UserEntity user, string name)
        {
            GuildEntity? guild = await _db.Guild.FirstOrDefaultAsync(x => x.User.ID == user.ID);
            if (guild != null)
            {
                throw new EntityAlreadyExistsException(nameof(GuildEntity), nameof(guild.Name), name);
            }

            EntityEntry<GuildEntity> guildEntry = await _db.Guild.AddAsync(new() { Name = name, Currency = 100, User = user,  });

            List<FeatEntity> featList = await _db.Feat.Where(x => x.RequirementList.Count() == 0).ToListAsync();
            featList.ForEach(async feat => await _db.GuildFeat.AddAsync(new() { Feat = feat, Guild = guildEntry.Entity }));

            List<RoleEntity> roleList = await _db.Role.Where(x => x.RequirementList.Count() == 0).ToListAsync();
            roleList.ForEach(async role => await _db.GuildRole.AddAsync(new() { Role = role, Guild = guildEntry.Entity }));

            await _db.SaveChangesAsync();

            return guildEntry.Entity;
        }

        public async Task<UnitEntity> InitiateUnit(Guid userID, Guid unitID)
        {
            GuildEntity guild = await GetGuild(userID);
            UnitEntity unit = await GetFullUnit(unitID, guild.ID);

            if (unit.Recruited)
            {
                throw new EntityStateException(userID, unit);
            }

            guild.Currency -= unit.GetCost();
            if (guild.Currency < 0)
            {
                throw new NotEnoughCurrencyException(guild.Currency);
            }

            unit.Recruited = true;
            await _db.SaveChangesAsync();

            return unit;
        }

        public async Task<MissionEntity> InitiateMission(Guid userID, Guid missionID, List<Guid> listUnitID)
        {
            GuildEntity guild = await GetGuild(userID);
            MissionEntity mission = await GetFullMission(missionID, guild.ID);

            if (mission.TimeStarted != null)
            {
                throw new EntityStateException(userID, mission);
            }

            List<UnitEntity> listUnit = await _db.Unit.Where(x => listUnitID.Contains(x.ID) && x.Guild.ID == guild.ID).ToListAsync();
            List<Guid> listMissingUnit = listUnitID.Except(listUnit.Select(x => x.ID)).ToList();

            if (listMissingUnit.Count > 0)
            {
                // Should log missing units.
                throw new EntityNotFoundException(userID);
            }

            mission.UnitList = listUnit;
            mission.TimeStarted = DateTime.UtcNow;
            await _db.SaveChangesAsync();

            return mission;
        }

        public async Task<List<UnitEntity>> RefreshAvailableUnitList(Guid userID)
        {
            GuildEntity guild = await _db.Guild
                .Include(x => x.UnitList).ThenInclude(x => x.UnitRoleList).ThenInclude(x => x.Role).ThenInclude(x => x.RoleType)
                .Include(x => x.UnitList).ThenInclude(x => x.UnitType).ThenInclude(x => x.EquipmentSlotList)
                .Include(x => x.GuildFeatList).ThenInclude(x => x.Feat).ThenInclude(x => x.GuildModifierList)
                .Include(x => x.GuildRoleList).ThenInclude(x => x.Role).ThenInclude(x => x.RoleType)
                .FirstOrDefaultAsync(x => x.User.ID == userID)
                ?? throw new EntityNotFoundException(userID);

            int refreshTotal = (int)guild.GetModifierValue<GuildUnitModifierEntity>(_config.Game.UnitListRefresh, x => x.Tag == GuildUnitModifierTagType.RefreshTime);
            if (guild.TimeUnitRefresh != null && guild.TimeUnitRefresh?.AddSeconds(refreshTotal) >= DateTime.UtcNow)
            {
                return guild.UnitList;
            }

            guild.TimeUnitRefresh = DateTime.UtcNow;
            guild.UnitList = guild.UnitList.Where(x => x.Recruited).ToList();

            int countTotal = (int)guild.GetUnitModifierValue(GuildUnitModifierTagType.Count, _config.Game.UnitListCount);
            int experienceTotal = (int)guild.GetUnitModifierValue(GuildUnitModifierTagType.Experience);

            GuildRoleModifierEntity.Set current = guild.GetRoleModifierSet();
            Dictionary<Guid, GuildRoleModifierEntity.Set> dictRole = await _db.RoleType.ToDictionaryAsync(x => x.ID, x => guild.GetRoleModifierSet(x.ID));
            for (int i = 0; i < countTotal; i++)
            {
                EntityEntry<UnitEntity> unitEntry = await _db.Unit.AddAsync(new()
                {
                    Name = $"Recruitable Unit #{i + 1}",
                    Experience = RNG.IntBetweenFactors(experienceTotal, 0.9f, 1.1f),
                    Recruited = false,
                    Guild = guild,
                    UnitType = 
                    UnitRoleList = new List<UnitRoleEntity>(),
                });

                Stack<int> splitCount = RNG.SplitIntRandomly(current.Count, dictRole.Count);
                foreach (KeyValuePair<Guid, GuildRoleModifierEntity.Set> pair in dictRole)
                {
                    int nextCount = splitCount.Pop();
                    int totalCount = pair.Value.Count + nextCount;
                    int totalExperience = RNG.IntBetweenFactors(current.Experience * nextCount + pair.Value.Experience * pair.Value.Count, 0.9f, 1.1f);

                    Stack<int> splitExperience = RNG.SplitIntRandomly(totalExperience, totalCount);
                    IEnumerable<GuildRoleEntity> roleList = guild.GuildRoleList.Where(x => x.Role.RoleType.ID == pair.Key);
                    IEnumerable<GuildRoleEntity> randomList = RNG.GetRandomElementList(roleList, totalCount);
                    foreach (GuildRoleEntity item in randomList)
                    {
                        EntityEntry<UnitRoleEntity> roleLevelEntry = await _db.UnitRole.AddAsync(new()
                        {
                            Experience = splitExperience.Pop(),
                            Unit = unitEntry.Entity,
                            Role = item.Role,
                        });
                    }
                }
            }

            await _db.SaveChangesAsync();

            return guild.UnitList;
        }

        public async Task<List<MissionEntity>> RefreshAvailableMissionList(Guid userID)
        {
            GuildEntity guild = await _db.Guild
                .Include(x => x.MissionList).ThenInclude(x => x.UnitList).ThenInclude(x => x.UnitRoleList).ThenInclude(x => x.Role).ThenInclude(x => x.RoleType)
                .Include(x => x.GuildFeatList).ThenInclude(x => x.Feat).ThenInclude(x => x.GuildModifierList)
                .Include(x => x.GuildRoleList).ThenInclude(x => x.Role).ThenInclude(x => x.RoleType)
                .FirstOrDefaultAsync(x => x.User.ID == userID)
                ?? throw new EntityNotFoundException(userID);

            int countTotal = (int)guild.GetModifierValue<GuildMissionModifierEntity>(_config.Game.MissionListCount, x => x.Tag == GuildMissionModifierTagType.Count);
            int refreshTotal = (int)guild.GetModifierValue<GuildMissionModifierEntity>(_config.Game.MissionListRefresh, x => x.Tag == GuildMissionModifierTagType.RefreshTime);
            if (guild.TimeMissionRefresh != null && guild.TimeMissionRefresh?.AddSeconds(refreshTotal) >= DateTime.UtcNow)
            {
                return guild.MissionList;
            }

            guild.TimeMissionRefresh = DateTime.UtcNow;
            guild.MissionList = guild.MissionList.Where(x => x.TimeStarted != null).ToList();

            int roleCount = (int)Math.Floor(guild.GuildRoleList.Count * _config.Game.MissionListRoleRatio);
            for (int i = 0; i < countTotal; i++)
            {
                List<RoleEntity> roleList = RNG.GetRandomElementList(guild.GuildRoleList, RNG.NextInt(1, roleCount)).Select(x => x.Role).ToList();
                EntityEntry<MissionEntity> missionEntry = await _db.Mission.AddAsync(new() { Guild = guild, RoleList = roleList });
            }

            await _db.SaveChangesAsync();

            return guild.MissionList;
        }

        #region -- Private utility methods --

        private async Task<GuildEntity> GetGuild(Guid userID)
        {
            return await _db.Guild.FirstOrDefaultAsync(x => x.User.ID == userID)
                ?? throw new EntityNotFoundException(userID);
        }

        private async Task<UnitEntity> GetFullUnit(Guid unitID, Guid guildID)
        {
            return await _db.Unit
                .Include(x => x.UnitRoleList).ThenInclude(x => x.Role).ThenInclude(x => x.RoleType)
                .FirstOrDefaultAsync(x => x.ID == unitID && x.Guild.ID == guildID)
                ?? throw new EntityNotFoundException(unitID);
        }

        private async Task<MissionEntity> GetFullMission(Guid missionID, Guid guildID)
        {
            return await _db.Mission
                .Include(x => x.UnitList).ThenInclude(x => x.UnitRoleList).ThenInclude(x => x.Role).ThenInclude(x => x.RoleType)
                .FirstOrDefaultAsync(x => x.ID == missionID && x.Guild.ID == guildID)
                ?? throw new EntityNotFoundException(missionID);
        }

        #endregion  -- Private utility methods --

    }
}
