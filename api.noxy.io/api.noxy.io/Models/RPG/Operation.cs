using Database.Enums;
using Database.Models.RPG.Abstract;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Models.RPG
{
    [Table("Operation")]
    public class Operation
    {
        [Key]
        public Guid ID { get; set; } = Guid.NewGuid();

        public string? Description { get; set; }

        [Required]
        public required TargetTagType TargetTag { get; set; }

        public List<Abstract.Action> ActionList { get; set; } = new();

        public List<Modifier> ModifierList { get; set; } = new();

        [Required]
        public DateTime TimeCreated { get; set; } = DateTime.UtcNow;

        #region Mappings

        public List<Abstract.Trigger> TriggerList { get; set; } = new();

        #endregion Mappings
    }
}
