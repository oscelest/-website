using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.noxy.io.Models.RPG
{
    [Table("Guild")]
    public class Guild
    {
        [Key]
        public Guid ID { get; set; } = Guid.NewGuid();

        [Required]
        public required string Name { get; set; }

        [Required]
        public required User User { get; set; }  

        [Required]
        public DateTime TimeCreated { get; set; } = DateTime.UtcNow;
    }
}
