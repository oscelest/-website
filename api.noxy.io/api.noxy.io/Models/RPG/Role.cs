using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Models.RPG
{
    [Table("Role")]
    public class Role
    {
        [Key]
        public Guid ID { get; set; } = Guid.NewGuid();

        [Required]
        public required TemplateRole TemplateRole { get; set; }

        [Required]
        public required Unit Unit { get; set; }

        [Required]
        public required int Experience { get; set; }

        [Required]
        public DateTime TimeCreated { get; set; } = DateTime.UtcNow;
    }
}
