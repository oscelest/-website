using api.noxy.io.Context;
using api.noxy.io.Exceptions;
using api.noxy.io.Models.Auth;
using api.noxy.io.Utility;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace api.noxy.io.Interface
{
    public interface IUserRepository
    {
        public Task<UserEntity> FindOne(Guid id);
        public Task<UserEntity> FindOne(string email);
        public Task<UserEntity> Create(string email, string password);
    }

    public class UserRepository : IUserRepository
    {
        private readonly APIContext _db;

        public UserRepository(APIContext db)
        {
            _db = db;
        }

        public async Task<UserEntity> FindOne(Guid id)
        {
            return await _db.User!.FindAsync(id) 
                ?? throw new EntityNotFoundException(id);
        }

        public async Task<UserEntity> FindOne(string email)
        {
            return await _db.User!.FirstOrDefaultAsync(e => e.Email == email) 
                ?? throw new EntityNotFoundException(Guid.Empty);
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
