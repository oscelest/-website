using api.noxy.io.Context;
using api.noxy.io.Models;
using api.noxy.io.Models.Game.Base;
using api.noxy.io.Models.Game.Modifier;
using Microsoft.EntityFrameworkCore;

namespace api.noxy.io.Interface
{
    public interface IGuildRepository
    {
        public Task<GuildEntity?> FindByID(Guid id);
        public Task<GuildEntity?> FindByUser(UserEntity userEntity);
        public Task<GuildEntity> Create(string name, UserEntity user);
        public Task<GuildEntity> RefreshRecruitment(UserEntity user);
    }

    public class GuildRepository : IGuildRepository
    {
        private readonly APIContext _db;

        public GuildRepository(APIContext db)
        {
            _db = db;
        }

        public async Task<GuildEntity?> FindByID(Guid id)
        {
            return await _db.Guild.FindAsync(id);
        }

        public async Task<GuildEntity?> FindByUser(UserEntity user)
        {
            return await _db.Guild.FirstOrDefaultAsync(e => e.User.ID == user.ID);
        }

        public async Task<GuildEntity> Create(string name, UserEntity user)
        {
            GuildEntity entity = new() { Name = name, User = user };
            await _db.Guild.AddAsync(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

        public async Task<GuildEntity> RefreshRecruitment(UserEntity user)
        {
            var guild = await _db.Guild.Include(x => x.FeatList).ThenInclude(x => x.RecruitmentModifierList).FirstOrDefaultAsync(e => e.User.ID == user.ID);
            if (guild == null) throw new Exception();

            RecruitmentModifierEntity[] refresh_rate = guild.FeatList.SelectMany(x => x.RecruitmentModifierList.Where(y => y.Tag == RecruitmentModifierEntity.TagType.RefreshRate)).ToArray();

            var offset_additive = refresh_rate.Where(x => x.Arithmatical == RecruitmentModifierEntity.ArithmaticalType.Additive).Sum(x => x.Value);
            var offset_multiplicative = refresh_rate.Where(x => x.Arithmatical == RecruitmentModifierEntity.ArithmaticalType.Multiplicative).Sum(x => x.Value);
            var offset_exponential = refresh_rate.Where(x => x.Arithmatical == RecruitmentModifierEntity.ArithmaticalType.Exponential).Select(x => x.Value).Aggregate(1, (a, x) => a * x);

            var offset = 60000 - (offset_additive * (1 + offset_multiplicative)) * offset_exponential;

            if (guild.TimeRecruitment.AddSeconds(offset) > DateTime.UtcNow)
            {
                return guild;
            }

            guild.TimeRecruitment = DateTime.UtcNow;
            await _db.SaveChangesAsync();

            return guild;
        }
    }
}
