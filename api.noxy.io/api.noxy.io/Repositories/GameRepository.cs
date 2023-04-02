using api.noxy.io.Context;
using api.noxy.io.Exceptions;
using api.noxy.io.Models.Game.Guild;
using api.noxy.io.Utilities;
using Microsoft.EntityFrameworkCore;
using System.Data;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using api.noxy.io.Models.Auth;

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
        private readonly IConfiguration _config;

        public GameRepository(APIContext db, IConfiguration config)
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
                 .Include(x => x.RoleLevelList).ThenInclude(x => x.Role).ThenInclude(x => x.RoleType)
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
                .Include(x => x.UnitList).ThenInclude(x => x.RoleLevelList).ThenInclude(x => x.Role).ThenInclude(x => x.RoleType)
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

            EntityEntry<GuildEntity> guildEntry = await _db.Guild.AddAsync(new()
            {
                Name = name,
                Currency = 100,
                User = user,
                UnitList = new(),
                MissionList = new(),
                GuildFeatList = new(),
            });

            List<FeatEntity> featList = await _db.Feat.Where(x => x.RequirementList.Count() == 0).ToListAsync();
            foreach (FeatEntity feat in featList)
            {
                await _db.GuildFeat.AddAsync(
                    new GuildFeatEntity
                    {
                        Feat = feat,
                        Guild = guildEntry.Entity,
                    }
                );
            }

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
                .Include(x => x.UnitList).ThenInclude(x => x.RoleLevelList).ThenInclude(x => x.Role).ThenInclude(x => x.RoleType)
                .Include(x => x.GuildFeatList).ThenInclude(x => x.Feat).ThenInclude(x => x.GuildModifierList)
                .FirstOrDefaultAsync(x => x.User.ID == userID)
                ?? throw new EntityNotFoundException(userID);

            int refreshBase = int.TryParse(_config["Game:UnitListRefresh"], out int refreshConfig) ? refreshConfig : 0;
            float refreshTotal = guild.GetModifierValue<GuildUnitModifierEntity>(refreshBase, x => x.Tag == GuildUnitModifierTagType.RefreshTime);
            if (guild.TimeUnitRefresh != null && guild.TimeUnitRefresh?.AddSeconds(refreshTotal) >= DateTime.UtcNow)
            {
                return guild.UnitList;
            }

            guild.TimeUnitRefresh = DateTime.UtcNow;
            guild.UnitList = guild.UnitList.Where(x => x.Recruited).ToList();

            int countBase = int.TryParse(_config["Game:UnitListCount"], out int countConfig) ? countConfig : 0;
            int countTotal = (int)guild.GetModifierValue<GuildUnitModifierEntity>(countBase, x => x.Tag == GuildUnitModifierTagType.Count);
            int experienceTotal = (int)guild.GetModifierValue<GuildUnitModifierEntity>(x => x.Tag == GuildUnitModifierTagType.Experience);

            // TODO: Should load your available roles
            List<RoleEntity> role_list = await _db.Role.ToListAsync();
            Dictionary<Guid, IEnumerable<RoleEntity>> role_dict = new()
            {
                { Guid.Empty, role_list }
            };
            Dictionary<Guid, int> role_type_dict = new()
            {
                { Guid.Empty, (int)guild.GetModifierValue<GuildRoleModifierEntity>(x => x.RoleType == null && x.Tag == GuildRoleModifierTagType.Count) }
            };

            foreach (RoleTypeEntity type in await _db.RoleType.ToListAsync())
            {
                role_dict.Add(type.ID, role_list.Where(x => x.RoleType.ID == type.ID));
                role_type_dict.Add(type.ID, (int)guild.GetModifierValue<GuildRoleModifierEntity>(x => x.RoleType.ID == type.ID && x.Tag == GuildRoleModifierTagType.Count));
            }

            for (int i = 0; i < countTotal; i++)
            {
                EntityEntry<UnitEntity> unitEntry = await _db.Unit.AddAsync(new()
                {
                    Name = $"Recruitable Unit #{i + 1}",
                    Experience = RNG.IntBetweenFactors(experienceTotal),
                    Recruited = false,
                    Guild = guild,
                    RoleLevelList = new List<RoleLevelEntity>(),
                });

                foreach (KeyValuePair<Guid, int> pair in role_type_dict)
                {
                    foreach (RoleEntity role in RNG.GetRandomElementList(role_dict[pair.Key], pair.Value))
                    {
                        EntityEntry<RoleLevelEntity> roleLevelEntry = await _db.RoleLevel.AddAsync(new()
                        {
                            Experience = 0,
                            Unit = unitEntry.Entity,
                            Role = role,
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
                .Include(x => x.MissionList).ThenInclude(x => x.UnitList).ThenInclude(x => x.RoleLevelList).ThenInclude(x => x.Role).ThenInclude(x => x.RoleType)
                .Include(x => x.GuildFeatList).ThenInclude(x => x.Feat).ThenInclude(x => x.GuildModifierList)
                .FirstOrDefaultAsync(x => x.User.ID == userID)
                ?? throw new EntityNotFoundException(userID);

            int countBase = int.TryParse(_config["Game:MissionListCount"], out int countConfig) ? countConfig : 0;
            int refreshBase = int.TryParse(_config["Game:MissionListRefresh"], out int refreshConfig) ? refreshConfig : 0;

            int countTotal = (int)guild.GetModifierValue<GuildMissionModifierEntity>(countBase, x => x.Tag == GuildMissionModifierTagType.Count) - guild.MissionList.Count(x => x.TimeStarted == null);
            float refreshTotal = guild.GetModifierValue<GuildMissionModifierEntity>(refreshBase, x => x.Tag == GuildMissionModifierTagType.RefreshTime);
            if (countTotal <= 0 || (guild.TimeMissionRefresh != null && guild.TimeMissionRefresh?.AddSeconds(refreshTotal) >= DateTime.UtcNow))
            {
                return guild.MissionList;
            }

            guild.TimeMissionRefresh = DateTime.UtcNow;
            guild.MissionList = guild.MissionList.Where(x => x.TimeStarted != null).ToList();

            // TODO: Should load your available roles
            List<RoleEntity> role_list = await _db.Role.ToListAsync();
            for (int i = 0; i < countTotal; i++)
            {
                EntityEntry<MissionEntity> missionEntry = await _db.Mission.AddAsync(new()
                {
                    BaseDuration = 100,
                    Guild = guild,
                });

                guild.MissionList.Add(missionEntry.Entity);
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
                .Include(x => x.RoleLevelList).ThenInclude(x => x.Role).ThenInclude(x => x.RoleType)
                .FirstOrDefaultAsync(x => x.ID == unitID && x.Guild.ID == guildID)
                ?? throw new EntityNotFoundException(unitID);
        }

        private async Task<MissionEntity> GetFullMission(Guid missionID, Guid guildID)
        {
            return await _db.Mission
                .Include(x => x.UnitList).ThenInclude(x => x.RoleLevelList).ThenInclude(x => x.Role).ThenInclude(x => x.RoleType)
                .FirstOrDefaultAsync(x => x.ID == missionID && x.Guild.ID == guildID)
                ?? throw new EntityNotFoundException(missionID);
        }

        #endregion  -- Private utility methods --

    }
}
