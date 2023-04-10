using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.noxy.io.Models.RPG
{
    [Table("Slot")]
    public class Slot
    {
        [Key]
        public Guid ID { get; set; } = Guid.NewGuid();

        [Required]
        public required string Name { get; set; }
    }
}
