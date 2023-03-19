using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Security.Cryptography;
using System.Text;

namespace api.noxy.io.Models
{
    public class UserEntity : SimpleEntity
    {
        private static readonly int keysize = 64;
        private static readonly int iterations = 100000;
        private static readonly HashAlgorithmName algorithm = HashAlgorithmName.SHA512;

        public string Email { get; set; } = string.Empty;
        public byte[] Salt { get; set; } = Array.Empty<byte>();
        public byte[] Hash { get; set; } = Array.Empty<byte>();

        public static byte[] GenerateSalt()
        {
            return RandomNumberGenerator.GetBytes(64);
        }

        public static byte[] GenerateHash(string password, byte[] salt)
        {
            return Rfc2898DeriveBytes.Pbkdf2(Encoding.UTF8.GetBytes(password), salt, iterations, algorithm, keysize);
        }

        new public DTO ToDTO() => new(this);

        public DTO ToDTO(string token) => new(this, token);

        public static void AddTableToBuilder(EntityTypeBuilder<UserEntity> builder)
        {
            builder.ToTable(nameof(UserEntity));
            builder.Property(x => x.Email).IsRequired();
            builder.Property(x => x.Salt).IsRequired();
            builder.Property(x => x.Hash).IsRequired();
            builder.HasIndex(x => x.Email).IsUnique();
        }

        new public class DTO : SimpleEntity.DTO
        {
            public string Email { get; set; }
            public string? Token { get; set; }

            public DTO(UserEntity entity) : base(entity)
            {
                Email = entity.Email;
            }

            public DTO(UserEntity entity, string token) : base(entity)
            {
                Email = entity.Email;
                Token = token;
            }
        }
    }
}
