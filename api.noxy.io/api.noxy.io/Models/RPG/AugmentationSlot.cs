using api.noxy.io.Models.RPG.Junction;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.noxy.io.Models.RPG
{
    [Table("AugmentationSlot")]
    public class AugmentationSlot
    {
        [Key]
        public Guid ID { get; set; } = Guid.NewGuid();

        [Required]
        public required Slot Slot { get; set; }

        [Required]
        public required Item Item { get; set; }

        [Required]
        public DateTime TimeCreated { get; set; } = DateTime.UtcNow;

        // Mappings
        public required List<EquipmentWithAugmentationInSlot> EquipmentAugmentationSlotList { get; set; }
    }
}
