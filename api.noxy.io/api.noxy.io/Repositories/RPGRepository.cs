using api.noxy.io.Context;
using api.noxy.io.Exceptions;
using api.noxy.io.Utility;
using api.noxy.io.Models.RPG;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using api.noxy.io.Utilities;

namespace api.noxy.io.Interface
{
    public interface IRPGRepository
    {
        public Task<Guild> CreateGuild(User user, string name);
        public Task<Guild> LoadGuild(User user);
    }

    public class RPGRepository : IRPGRepository
    {
        private readonly RPGContext _db;
        private readonly IApplicationConfiguration _config;

        public RPGRepository(RPGContext db, IApplicationConfiguration config)
        {
            _db = db;
            _config = config;
        }

        public async Task<Guild> CreateGuild(User user, string name)
        {
            Guild? guild = await _db.Guild.FirstOrDefaultAsync(x => x.UserRef.ID == user.ID || x.Name == name);
            if (guild != null)
            {
                throw new EntityAlreadyExistsException();
            }

            EntityEntry<Guild> guildEntry = _db.Guild.Add(new Guild { Name = name, UserRef = user });
            _db.SaveChanges();

            return guildEntry.Entity;
        }

        public async Task<Guild> LoadGuild(User user)
        {
            return await _db.Guild.FirstOrDefaultAsync(x => x.UserRef.ID == user.ID) ?? throw new EntityNotFoundException();
        }

        public async Task<List<Unit>> LoadUnitInitiatedList(User user)
        {
            Guild guild = await _db.Guild.FirstOrDefaultAsync(x => x.UserRef.ID == user.ID) ?? throw new EntityNotFoundException();

            EventGuild? entityEventGuild = await _db.EventGuild
                .Where(x => x.Guild.ID == guild.ID && x.Tag == EventTagType.UnitRefresh)
                .FirstOrDefaultAsync(x => x.TimeLastOccurrence == _db.EventGuild.Max(y => y.TimeLastOccurrence));
            
            List<ModifierGuildUnit> listModifierGuildUnit = await _db.ModifierGuildUnit
                .Where(x => x.UnitTag == ModifierGuildUnitTagType.RefreshTime)
                .ToListAsync();


            int refreshTotal = (int)guild.GetModifierValue<GuildUnitModifierEntity>(_config.Game.UnitListRefresh, x => x.Tag == ModifierGuildUnitTagType.RefreshTime);

            if (guild.TimeUnitRefresh != null && guild.TimeUnitRefresh?.AddSeconds(refreshTotal) >= DateTime.UtcNow)
            {
                throw new EntityStateException();
            }

            _db.Unit.RemoveRange(await _db.Unit.Where(x => x.Guild.ID == guild.ID && x.TimeInitiated == null).ToListAsync());



            //int refreshTotal = (int)guild.GetModifierValue<GuildUnitModifierEntity>(_config.Game.UnitListRefresh, x => x.Tag == ModifierGuildUnitTagType.RefreshTime);
            //if (guild.TimeUnitRefresh != null && guild.TimeUnitRefresh?.AddSeconds(refreshTotal) >= DateTime.UtcNow)
            //{
            //    return guild.UnitList;
            //}

            //guild.TimeUnitRefresh = DateTime.UtcNow;
            //guild.UnitList = guild.UnitList.Where(x => x.Recruited).ToList();

            //int countTotal = (int)guild.GetUnitModifierValue(ModifierGuildUnitTagType.Count, _config.Game.UnitListCount);
            //int experienceTotal = (int)guild.GetUnitModifierValue(ModifierGuildUnitTagType.Experience);

            //GuildRoleModifierEntity.Set current = guild.GetRoleModifierSet();
            //List<UnitTypeEntity> listUnitType = await _db.UnitType.ToListAsync();
            //Dictionary<Guid, GuildRoleModifierEntity.Set> dictRole = await _db.RoleType.ToDictionaryAsync(x => x.ID, x => guild.GetRoleModifierSet(x.ID));
            //for (int i = 0; i < countTotal; i++)
            //{
            //    EntityEntry<UnitTypeEntity> unitEntry = await _db.Unit.AddAsync(new()
            //    {
            //        Name = $"Recruitable Unit #{i + 1}",
            //        Experience = RNG.IntBetweenFactors(experienceTotal, 0.9f, 1.1f),
            //        Recruited = false,
            //        Guild = guild,
            //        UnitType = RNG.GetRandomElement(listUnitType),
            //    });

            //    Stack<int> splitCount = RNG.SplitIntRandomly(current.Count, dictRole.Count);
            //    foreach (KeyValuePair<Guid, GuildRoleModifierEntity.Set> pair in dictRole)
            //    {
            //        int nextCount = splitCount.Pop();
            //        int totalCount = pair.Value.Count + nextCount;
            //        int totalExperience = RNG.IntBetweenFactors(current.Experience * nextCount + pair.Value.Experience * pair.Value.Count, 0.9f, 1.1f);

            //        Stack<int> splitExperience = RNG.SplitIntRandomly(totalExperience, totalCount);
            //        IEnumerable<GuildRoleEntity> roleList = guild.GuildRoleList.Where(x => x.Role.RoleType.ID == pair.Key);
            //        IEnumerable<GuildRoleEntity> randomList = RNG.GetRandomElementList(roleList, totalCount);
            //        foreach (GuildRoleEntity item in randomList)
            //        {
            //            EntityEntry<UnitRoleEntity> roleLevelEntry = await _db.UnitRole.AddAsync(new()
            //            {
            //                Experience = splitExperience.Pop(),
            //                Unit = unitEntry.Entity,
            //                Role = item.Role,
            //            });
            //        }
            //    }
            //}

            //await _db.SaveChangesAsync();

            //return guild.UnitList;


            //_config.Game.UnitListRefresh



            return new List<Unit> { new Unit() { Experience = 0, Guild = guild, TemplateUnit = new TemplateUnit() { Name = "" } } };
        }

    }
}
