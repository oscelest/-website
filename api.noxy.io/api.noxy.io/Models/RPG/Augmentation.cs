using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.noxy.io.Models.RPG
{
    [Table("Augmentation")]
    public class Augmentation : Item
    {
        [Key]
        public Guid ID { get; set; } = Guid.NewGuid();

        [Required]
        public required string Name { get; set; }

        [Required]
        public required Slot Slot { get; set; }

        [Required]
        public DateTime TimeCreated { get; set; } = DateTime.UtcNow;
    }
}
