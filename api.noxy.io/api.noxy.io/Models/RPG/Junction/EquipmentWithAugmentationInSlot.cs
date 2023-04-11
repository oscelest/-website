using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.noxy.io.Models.RPG.Junction
{
    [Table("$EquipmentWithAugmentationInSlot")]
    [PrimaryKey(nameof(ItemEquipment), nameof(ItemAugmentation), nameof(SlotAugmentation))]
    public class EquipmentWithAugmentationInSlot
    {
        [Required]
        public required Item.Equipment ItemEquipment { get; set; }

        [Required]
        public required Item.Augmentation ItemAugmentation { get; set; }

        [Required]
        public required Slot.Augmentation SlotAugmentation { get; set; }

        [Required]
        public DateTime TimeCreated { get; set; } = DateTime.UtcNow;
    }
}
