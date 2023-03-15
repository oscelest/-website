using api.noxy.io.Context;
using api.noxy.io.Models;
using Microsoft.EntityFrameworkCore;

namespace api.noxy.io.Interface
{
    public interface IUserRepository
    {
        public Task<UserEntity?> FindByID(Guid id);
        public Task<UserEntity?> FindByEmail(string email);
        public Task<UserEntity> Create(string email, string password);
    }

    public class UserRepository : IUserRepository
    {
        private readonly APIContext _db;

        public UserRepository(APIContext db)
        {
            _db = db;
        }

        public async Task<UserEntity?> FindByID(Guid id)
        {
            return await _db.User!.FindAsync(id);
        }

        public async Task<UserEntity?> FindByEmail(string email)
        {
            return await _db.User!.FirstOrDefaultAsync(e => e.Email == email);
        }

        public async Task<UserEntity> Create(string email, string password)
        {
            var salt = UserEntity.GenerateSalt();
            var hash = UserEntity.GenerateHash(password, salt);
            var user = new UserEntity() { Email = email, Salt = salt, Hash = hash };

            await _db.User!.AddAsync(user);
            await _db.SaveChangesAsync();
            return user;
        }

    }
}
