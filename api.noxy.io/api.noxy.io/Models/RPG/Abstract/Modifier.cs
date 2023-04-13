using api.noxy.io.Utilities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.noxy.io.Models.RPG
{
    public abstract class Modifier
    {
        [Key]
        public Guid ID { get; set; } = Guid.NewGuid();

        [Required]
        public required float Value { get; set; }

        [Required]
        [Column(TypeName = "varchar(32)")]
        public required ArithmeticalTagType ArithmeticalTag { get; set; }

        [Required]
        public DateTime TimeCreated { get; set; } = DateTime.UtcNow;
    }
}
