using Database.Enums;
using System.ComponentModel.DataAnnotations;

namespace Database.Models.RPG.Abstract
{
    public abstract class Action
    {
        [Key]
        public Guid ID { get; set; } = Guid.NewGuid();

        [Required]
        public required ActionTagType ActionTag { get; set; }

        public List<Modifier> ModifierList { get; set; } = new();

        [Required]
        public DateTime TimeCreated { get; set; } = DateTime.UtcNow;
    }
}
