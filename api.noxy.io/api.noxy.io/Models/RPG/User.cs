using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography;
using System.Text;

namespace Database.Models.RPG
{
    [Table("User")]
    [Index(nameof(Email), IsUnique = true)]
    public class User
    {
        [Key]
        public Guid ID { get; set; } = Guid.NewGuid();

        [Required]
        public required string Email { get; set; }

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
    }
}
