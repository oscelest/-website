using api.noxy.io.Context;
using api.noxy.io.Exceptions;
using api.noxy.io.Models.Game.Guild;
using api.noxy.io.Utilities;
using Microsoft.EntityFrameworkCore;
using System.Data;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using api.noxy.io.Models.Auth;
using api.noxy.io.Utility;
using api.noxy.io.Models.Game.Item;
using api.noxy.io.Models.Game.Recipe;
using api.noxy.io.Models.Game.Mission;
using api.noxy.io.Models.Game.Feat;
using api.noxy.io.Models.Game.Unit;

namespace api.noxy.io.Interface
{
    public interface IGameRepository
    {
        public Task<GuildEntity?> LoadGuild(Guid userID);
        public Task<GuildEntity> CreateGuild(UserEntity user, string name);
        public Task<List<UnitTypeEntity>> LoadUnitList(Guid userID);
        public Task<List<UnitTypeEntity>> LoadUnitList(GuildEntity guild);
        public Task<List<MissionEntity>> LoadMissionList(Guid userID);
        public Task<List<MissionEntity>> LoadMissionList(GuildEntity guild);
        public Task<UnitTypeEntity> InitiateUnit(Guid userID, Guid unitID);
        public Task<MissionEntity> InitiateMission(Guid userID, Guid missionID, List<Guid> listUnitID);
        public Task<List<UnitTypeEntity>> RefreshAvailableUnitList(Guid userID);
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

        public async Task<List<UnitTypeEntity>> LoadUnitList(Guid userID)
        {
            return await LoadUnitList(await GetGuild(userID));
        }

        public async Task<List<UnitTypeEntity>> LoadUnitList(GuildEntity guild)
        {
            return await _db.Unit
                 .Include(x => x.UnitType).ThenInclude(x => x.EquipmentSlotList)
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
                throw new EntityAlreadyExistsException();
            }

            EntityEntry<GuildEntity> guildEntry = await _db.Guild.AddAsync(new() { Name = name, Currency = 100, User = user, });

            List<FeatEntity> featList = await _db.Feat.Where(x => x.RequirementList.Count() == 0).ToListAsync();
            featList.ForEach(async feat => await _db.GuildFeat.AddAsync(new() { Feat = feat, Guild = guildEntry.Entity }));

            List<GuildRoleEntity> roleList = await _db.Role.Where(x => x.RequirementList.Count() == 0).ToListAsync();
            roleList.ForEach(async role => await _db.GuildRole.AddAsync(new() { Role = role, Guild = guildEntry.Entity }));

            await _db.SaveChangesAsync();

            return guildEntry.Entity;
        }

        public async Task<UnitTypeEntity> InitiateUnit(Guid userID, Guid unitID)
        {
            GuildEntity guild = await GetGuild(userID);
            UnitTypeEntity unit = await GetFullUnit(unitID, guild.ID);

            if (unit.Recruited)
            {
                throw new EntityStateException();
            }

            guild.Currency -= unit.GetCost();
            if (guild.Currency < 0)
            {
                throw new NotEnoughCurrencyException();
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
                throw new EntityStateException();
            }

            List<UnitTypeEntity> listUnit = await _db.Unit.Where(x => listUnitID.Contains(x.ID) && x.Guild.ID == guild.ID).ToListAsync();
            List<Guid> listMissingUnit = listUnitID.Except(listUnit.Select(x => x.ID)).ToList();

            if (listMissingUnit.Count > 0)
            {
                // Should log missing units.
                throw new EntityNotFoundException();
            }

            mission.UnitList = listUnit;
            mission.TimeStarted = DateTime.UtcNow;
            await _db.SaveChangesAsync();

            return mission;
        }

        public async Task<List<UnitTypeEntity>> RefreshAvailableUnitList(Guid userID)
        {
            GuildEntity guild = await _db.Guild
                .Include(x => x.UnitList).ThenInclude(x => x.UnitRoleList).ThenInclude(x => x.Role).ThenInclude(x => x.RoleType)
                .Include(x => x.UnitList).ThenInclude(x => x.UnitType).ThenInclude(x => x.EquipmentSlotList)
                .Include(x => x.GuildFeatList).ThenInclude(x => x.Feat).ThenInclude(x => x.GuildModifierList)
                .Include(x => x.GuildRoleList).ThenInclude(x => x.Role).ThenInclude(x => x.RoleType)
                .FirstOrDefaultAsync(x => x.User.ID == userID)
                ?? throw new EntityNotFoundException();

            int refreshTotal = (int)guild.GetModifierValue<GuildUnitModifierEntity>(_config.Game.UnitListRefresh, x => x.Tag == ModifierGuildUnitTagType.RefreshTime);
            if (guild.TimeUnitRefresh != null && guild.TimeUnitRefresh?.AddSeconds(refreshTotal) >= DateTime.UtcNow)
            {
                return guild.UnitList;
            }

            guild.TimeUnitRefresh = DateTime.UtcNow;
            guild.UnitList = guild.UnitList.Where(x => x.Recruited).ToList();

            int countTotal = (int)guild.GetUnitModifierValue(ModifierGuildUnitTagType.Count, _config.Game.UnitListCount);
            int experienceTotal = (int)guild.GetUnitModifierValue(ModifierGuildUnitTagType.Experience);

            GuildRoleModifierEntity.Set current = guild.GetRoleModifierSet();
            List<UnitTypeEntity> listUnitType = await _db.UnitType.ToListAsync();
            Dictionary<Guid, GuildRoleModifierEntity.Set> dictRole = await _db.RoleType.ToDictionaryAsync(x => x.ID, x => guild.GetRoleModifierSet(x.ID));
            for (int i = 0; i < countTotal; i++)
            {
                EntityEntry<UnitTypeEntity> unitEntry = await _db.Unit.AddAsync(new()
                {
                    Name = $"Recruitable Unit #{i + 1}",
                    Experience = RNG.IntBetweenFactors(experienceTotal, 0.9f, 1.1f),
                    Recruited = false,
                    Guild = guild,
                    UnitType = RNG.GetRandomElement(listUnitType),
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
                ?? throw new EntityNotFoundException();

            int countTotal = (int)guild.GetModifierValue<GuildMissionModifierEntity>(_config.Game.MissionListCount, x => x.Tag == ModifierGuildMissionTagType.Count);
            int refreshTotal = (int)guild.GetModifierValue<GuildMissionModifierEntity>(_config.Game.MissionListRefresh, x => x.Tag == ModifierGuildMissionTagType.RefreshTime);
            if (guild.TimeMissionRefresh != null && guild.TimeMissionRefresh?.AddSeconds(refreshTotal) >= DateTime.UtcNow)
            {
                return guild.MissionList;
            }

            guild.TimeMissionRefresh = DateTime.UtcNow;
            guild.MissionList = guild.MissionList.Where(x => x.TimeStarted != null).ToList();

            int roleCount = (int)Math.Floor(guild.GuildRoleList.Count * _config.Game.MissionListRoleRatio);
            for (int i = 0; i < countTotal; i++)
            {
                List<GuildRoleEntity> roleList = RNG.GetRandomElementList(guild.GuildRoleList, RNG.NextInt(1, roleCount)).Select(x => x.Role).ToList();
                EntityEntry<MissionEntity> missionEntry = await _db.Mission.AddAsync(new() { Guild = guild, RoleList = roleList });
            }

            await _db.SaveChangesAsync();

            return guild.MissionList;
        }

        public async Task RecipeCraft(Guid userID, Guid recipeID)
        {
            GuildEntity entityGuild = await _db.Guild.FirstOrDefaultAsync(x => x.User.ID == userID)
                ?? throw new EntityNotFoundException();

            GuildRecipeEntity entityGuildRecipe = await _db.GuildRecipe
                .Include(x => x.Recipe).ThenInclude(x => x.Item)
                .Include(x => x.Recipe).ThenInclude(x => x.RecipeItemList).ThenInclude(x => x.Item)
                .FirstOrDefaultAsync(x => x.Recipe.ID == recipeID)
                ?? throw new EntityNotFoundException();

            List<GuildItemEntity> listGuildItem = new();
            foreach (RecipeItemEntity entityRecipeItem in entityGuildRecipe.Recipe.RecipeItemList)
            {
                GuildItemEntity entityGuildItem = await _db.GuildItem
                    .Include(x => x.Item)
                    .FirstOrDefaultAsync(x => x.Guild.ID == entityGuild.ID && x.Item.ID == entityRecipeItem.ID)
                     ?? throw new EntityNotFoundException();

                if (entityGuildItem.Count < entityRecipeItem.Count)
                {
                    throw new EntityStateException("We don't have any or not enough of the needed item.");
                }

                if (entityGuildItem.Item is EquipmentItemEntity)
                {
                    listGuildItem.Add(entityGuildItem);
                }
                else if (entityGuildItem.Item is MaterialItemEntity)
                {
                    entityGuildItem.Count -= entityRecipeItem.Count;
                }
            }

            foreach (GuildItemEntity entityGuildItem in listGuildItem)
            {
                // TODO: We should remove sockets but won't do this now
                _db.GuildItem.Remove(entityGuildItem);
            }

            if (entityGuildRecipe.Recipe.Item is EquipmentItemEntity entityEquipmentItem)
            {
                _db.GuildItem.Add(new() { Guild = entityGuild, Item = entityEquipmentItem, Count = 1 });
            }
            else if (entityGuildRecipe.Recipe.Item is MaterialItemEntity entityMaterialItem)
            {
                GuildItemEntity? entityGuildItem = await _db.GuildItem.FirstOrDefaultAsync(x => x.Guild.ID == entityGuild.ID && x.Item.ID == entityMaterialItem.ID);
                if (entityGuildItem == null)
                {
                    _db.GuildItem.Add(new() { Guild = entityGuild, Item = entityMaterialItem, Count = 1 });
                }
                else
                {
                    entityGuildItem.Count++;
                }
            }

            await _db.SaveChangesAsync();
        }


        public async Task EquipItem(Guid userID, Guid itemID, Guid unitID)
        {
            GuildEntity entityGuild = await _db.Guild.FirstOrDefaultAsync(x => x.User.ID == userID)
                ?? throw new EntityNotFoundException();

            GuildEquipmentItemEntity entityGuildEquipmentItem = await _db.GuildEquipmentItem
                .Include(x => x.Item.Slot)
                .FirstOrDefaultAsync(x => x.Guild.ID == entityGuild.ID && x.Item.ID ==  itemID)
                ?? throw new EntityNotFoundException();

            UnitEntity entityUnit = await _db.Unit
            .Include(x => x.UnitType).ThenInclude(x => x.EquipmentSlotList)
            .FirstOrDefaultAsync(x => x.ID == unitID && x.GuildUnitList.Any(x => x.Guild.ID == entityGuild.ID))
            ?? throw new EntityNotFoundException();

            entityGuildEquipmentItem.Unit = entityUnit;

            EquipmentItemEntity entityEquipmentItem = await _db.EquipmentItem
                .Include(x => x.Slot)
                .FirstOrDefaultAsync(x => x.ID == itemID && x.GuildItemList.Any(x => x.Guild.ID == entityGuild.ID))
                ?? throw new EntityNotFoundException();

        

            if (!entityUnit.HasEquipmentSlot(entityEquipmentItem.Slot))
            {
                throw new EntityStateException("Unit does not have an equipment slot to equip the given item.");
            }

            List<EquipmentItemEntity> listEquipmentItem = await _db.EquipmentItem
                      .Where(x => x.GuildItemList.Any(x => x.))
                      ?? throw new EntityNotFoundException();

            await _db.GuildUnitGuildItem.RemoveRange(_db.GuildUnitGuildItem.Where(x => x.GuildItem.));


            await _db.SaveChangesAsync();


            var a = equipment.Slot;

            EquipmentItemEntity a = entityGuildItem.Item;

            GuildUnitEntity entityUnit = await _db.GuildUnit
                .Include(x => x.Unit)
                .FirstOrDefaultAsync(x => x.Guild.ID == entityGuild.ID && x.Unit.ID == unitID)
                ?? throw new EntityNotFoundException();





            //GuildItemEntity entityItem = await _db.GuildItem
            //    .Include(x => x.Item)
            //    .FirstOrDefaultAsync(x => x.Guild.ID == entityGuild.ID && x.ID == itemID)
            //    ?? throw new EntityNotFoundException();

            //entityUnit.UnitType.EquipmentSlotList.
        }


        #region -- Private utility methods --

        private async Task<GuildEntity> GetGuild(Guid userID)
        {
            return await _db.Guild.FirstOrDefaultAsync(x => x.User.ID == userID)
                ?? throw new EntityNotFoundException();
        }

        private async Task<UnitTypeEntity> GetFullUnit(Guid unitID, Guid guildID)
        {
            return await _db.Unit
                .Include(x => x.UnitRoleList).ThenInclude(x => x.Role).ThenInclude(x => x.RoleType)
                .FirstOrDefaultAsync(x => x.ID == unitID && x.Guild.ID == guildID)
                ?? throw new EntityNotFoundException();
        }

        private async Task<MissionEntity> GetFullMission(Guid missionID, Guid guildID)
        {
            return await _db.Mission
                .Include(x => x.UnitList).ThenInclude(x => x.UnitRoleList).ThenInclude(x => x.Role).ThenInclude(x => x.RoleType)
                .FirstOrDefaultAsync(x => x.ID == missionID && x.Guild.ID == guildID)
                ?? throw new EntityNotFoundException();
        }

        #endregion  -- Private utility methods --

    }
}
