using api.noxy.io.Context;
using api.noxy.io.Exceptions;
using api.noxy.io.Utility;
using api.noxy.io.Models.RPG;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

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

            return new List<Unit> { new Unit() { Experience = 0, Guild = guild, TemplateUnit = new TemplateUnit() { Name = "" } } };
        }

    }
}
