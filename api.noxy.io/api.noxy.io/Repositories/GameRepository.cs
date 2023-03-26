using api.noxy.io.Context;
using api.noxy.io.Models;
using api.noxy.io.Models.Game.Guild;
using api.noxy.io.Utilities;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace api.noxy.io.Interface
{
    public interface IGameRepository
    {
        public Task<GuildEntity?> FindByUser(UserEntity userEntity);
        public Task<GuildEntity> Create(string name, UserEntity user);
        public Task<GuildEntity> RefreshUnitList(UserEntity user);
        public Task<GuildEntity> RefreshMissionList(UserEntity user);
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

        public async Task<GuildEntity?> FindByUser(UserEntity user)
        {
            return await _db.Guild.FirstOrDefaultAsync(e => e.User.ID == user.ID);
        }

        public async Task<GuildEntity> Create(string name, UserEntity user)
        {
            GuildEntity entity = new() { 
                Name = name, 
                User = user, 
            };
            await _db.Guild.AddAsync(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

        public async Task<GuildEntity> RefreshUnitList(UserEntity user)
        {

            GuildEntity guild = await _db.Guild
                .Include(x => x.UnitList!)
                .Include(x => x.FeatList!).ThenInclude(x => x.GuildModifierList)
                .FirstOrDefaultAsync(e => e.User.ID == user.ID)
                ?? throw new Exception();

            int refreshBase = int.TryParse(_config["Game:UnitListRefresh"], out int refreshConfig) ? refreshConfig : 0;
            float refreshTotal = guild.GetModifierValue<GuildUnitModifierEntity>(refreshBase, x => x.Tag == GuildUnitModifierTagType.RefreshTime);
            if (guild.TimeUnitRefresh.AddSeconds(refreshTotal) >= DateTime.UtcNow)
            {
                return guild;
            }

            _db.Unit.RemoveRange(guild.UnitList.Where(x => !x.Recruited));
            guild.UnitList!.RemoveAll(x => !x.Recruited);
            guild.TimeUnitRefresh = DateTime.UtcNow;

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
                role_type_dict.Add(type.ID, (int)guild.GetModifierValue<GuildRoleModifierEntity>(x => x.RoleType?.ID == type.ID && x.Tag == GuildRoleModifierTagType.Count));
            }

            for (int i = 0; i < countTotal; i++)
            {
                UnitEntity unit = new()
                {
                    Name = $"Recruitable Unit #{i + 1}",
                    Experience = RNG.IntBetweenFactors(experienceTotal),
                };

                foreach (KeyValuePair<Guid, int> pair in role_type_dict)
                {
                    unit.RoleLevelList.AddRange(RNG.GetRandomElementList(role_dict[pair.Key], pair.Value).Select(role => new RoleLevelEntity()
                    {
                        Experience = 0,
                        Role = role
                    }));
                }

                guild.UnitList.Add(unit);
            }

            await _db.SaveChangesAsync();

            return guild;
        }

        public async Task<GuildEntity> RefreshMissionList(UserEntity user)
        {
            GuildEntity guild = await _db.Guild
                .Include(x => x.MissionList!)
                .Include(x => x.FeatList!).ThenInclude(x => x.GuildModifierList)
                .FirstOrDefaultAsync(e => e.User.ID == user.ID)
                ?? throw new Exception();

            int countBase = int.TryParse(_config["Game:MissionListCount"], out int countConfig) ? countConfig : 0;
            int refreshBase = int.TryParse(_config["Game:MissionListRefresh"], out int refreshConfig) ? refreshConfig : 0;

            int countTotal = (int)guild.GetModifierValue<GuildMissionModifierEntity>(countBase, x => x.Tag == GuildMissionModifierTagType.Count) - guild.MissionList.Count(x => x.Unit == null);
            float refreshTotal = guild.GetModifierValue<GuildMissionModifierEntity>(refreshBase, x => x.Tag == GuildMissionModifierTagType.RefreshTime);
            if (countTotal <= 0 || guild.TimeMissionRefresh.AddSeconds(refreshTotal) >= DateTime.UtcNow)
            {
                return guild;
            }

            _db.Mission.RemoveRange(guild.MissionList.Where(x => x.Unit == null));
            guild.MissionList.RemoveAll(x => x.Unit == null);
            guild.TimeMissionRefresh = DateTime.UtcNow;

            // TODO: Should load your available roles
            List<RoleEntity> role_list = await _db.Role.ToListAsync();

            for (int i = 0; i < countTotal; i++)
            {
                guild.MissionList.Add(new()
                {
                    BaseDuration = 100,
                    RoleList = RNG.GetRandomElementList(role_list, 1).ToList(),
                });
            }

            await _db.SaveChangesAsync();

            return guild;
        }
    }
}
