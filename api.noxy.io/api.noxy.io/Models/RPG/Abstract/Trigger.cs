using Database.Enums;
using System.ComponentModel.DataAnnotations;

namespace Database.Models.RPG.Abstract
{
    public abstract class Trigger
    {
        [Key]
        public Guid ID { get; set; } = Guid.NewGuid();

        [Required]
        public required TriggerTagType TriggerTag { get; set; }

        public List<Operation> OperationList { get; set; } = new();

        [Required]
        public DateTime TimeCreated { get; set; } = DateTime.UtcNow;
    }
}
