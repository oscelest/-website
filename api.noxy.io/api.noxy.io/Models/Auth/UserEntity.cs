using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography;
using System.Text;

namespace api.noxy.io.Models.Auth
{
    [Table("User")]
    [Index(nameof(Email), IsUnique = true)]
    public class UserEntity : SingleEntity
    {
        [Required]
        public string Email { get; set; } = string.Empty;

        [Required]
        public byte[] Salt { get; set; } = Array.Empty<byte>();

        [Required]
        public byte[] Hash { get; set; } = Array.Empty<byte>();

        public static byte[] GenerateSalt()
        {
            return RandomNumberGenerator.GetBytes(64);
        }

        public static byte[] GenerateHash(string password, byte[] salt)
        {
            return Rfc2898DeriveBytes.Pbkdf2(Encoding.UTF8.GetBytes(password), salt, 100000, HashAlgorithmName.SHA512, 64);
        }

        new public DTO ToDTO() => new(this);
        public DTO ToDTO(string token) => new(this, token);

        new public class DTO : SingleEntity.DTO
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
