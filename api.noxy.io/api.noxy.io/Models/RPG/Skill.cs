using Database.Enums;
using Database.Models.RPG.Abstract;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Models.RPG
{
    [Table("Skill")]
    public class Skill
    {
        [Key]
        public Guid ID { get; set; } = Guid.NewGuid();

        [Required]
        public required string Name { get; set; }

        [Required]
        public required SkillTagType SkillTag { get; set; }

        public List<Operation> OperationList { get; set; } = new();

        public List<Modifier> ModifierList { get; set; } = new();

        [Required]
        public DateTime TimeCreated { get; set; } = DateTime.UtcNow;
    }
}
