using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.noxy.io.Models.RPG
{
    [Table("Role")]
    public class Role
    {
        [Key]
        public Guid ID { get; set; } = Guid.NewGuid();

        [Required]
        public required string Name { get; set; }

        [Required]
        public DateTime TimeCreated = DateTime.UtcNow;
    }
}
