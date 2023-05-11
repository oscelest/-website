using Database.Contexts;
using Database.Exceptions;
using Database.Models.RPG;
using Microsoft.EntityFrameworkCore;

namespace Database.Repositories
{
    public interface IAuthRepository
    {
        public Task<User> FindOne(Guid id);
        public Task<User> FindOne(string email);
        public Task<User> Create(string email, string password);
    }

    public class AuthRepository : IAuthRepository
    {
        private readonly RPGContext _db;

        public AuthRepository(RPGContext db)
        {
            _db = db;
        }

        public async Task<User> FindOne(Guid id)
        {
            return await _db.User!.FindAsync(id)
                ?? throw new EntityNotFoundException<User>(id);
        }

        public async Task<User> FindOne(string email)
        {
            return await _db.User!.FirstOrDefaultAsync(e => e.Email == email)
                ?? throw new EntityNotFoundException<User>(email);
        }

        public async Task<User> Create(string email, string password)
        {
            byte[] salt = User.GenerateSalt();
            byte[] hash = User.GenerateHash(password, salt);
            User user = new() { Email = email, Salt = salt, Hash = hash };

            await _db.User!.AddAsync(user);
            await _db.SaveChangesAsync();
            return user;
        }
    }
}
