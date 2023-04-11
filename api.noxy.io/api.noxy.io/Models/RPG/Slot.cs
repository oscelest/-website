using api.noxy.io.Models.RPG.Junction;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.noxy.io.Models.RPG
{
    public abstract class Slot
    {
        [Key]
        public Guid ID { get; set; } = Guid.NewGuid();

        [Required]
        public required Template.Slot TemplateSlot { get; set; }

        [Required]
        public DateTime TimeCreated { get; set; } = DateTime.UtcNow;

        // Implementations

        [Table("SlotEquipment")]
        public class Equipment : Slot
        {
            [Required]
            public required Template.Unit TemplateUnit { get; set; }
        }

        [Table("SlotAugmentation")]
        public class Augmentation : Slot
        {
            [Required]
            public required Template.Item TemplateItem { get; set; }
        }
    }
}
