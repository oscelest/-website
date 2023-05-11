using Database.Enums;
using Database.Models.RPG.Abstract;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Models.RPG
{
    [Table("Effect")]
    public class Effect
    {
        [Key]
        public Guid ID { get; set; } = Guid.NewGuid();

        [Required]
        public required string Name { get; set; }

        [Required]
        public required bool IsExpirable { get; set; }

        [Required]
        public required bool IsRemovable { get; set; }

        [Required]
        public required EffectAlignmentTagType EffectAlignmentTag { get; set; }

        public List<Trigger> TriggerList { get; set; } = new();
        
        public List<Modifier> ModifierList { get; set; } = new();

        [Required]
        public DateTime TimeCreated { get; set; } = DateTime.UtcNow;
    }
}
