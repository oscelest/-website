using api.noxy.io.Context;
using api.noxy.io.Models;
using api.noxy.io.Models.Game;
using Microsoft.EntityFrameworkCore;

namespace api.noxy.io.Interface
{
    public interface IGuildRepository
    {
        public Task<GuildEntity?> FindByID(Guid id);
        public Task<GuildEntity?> FindByUser(UserEntity userEntity);
        public Task<GuildEntity> Create(string name, UserEntity user);
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
            return await _db.Guild!.FindAsync(id);
        }

        public async Task<GuildEntity?> FindByUser(UserEntity user)
        {
            return await _db.Guild!.FirstOrDefaultAsync(e => e.User.ID == user.ID);
        }

        public async Task<GuildEntity> Create(string name, UserEntity user)
        {
            GuildEntity entity = new() { Name = name, User = user };
            await _db.Guild!.AddAsync(entity);
            await _db.SaveChangesAsync();
            return entity;
        }
    }
}
