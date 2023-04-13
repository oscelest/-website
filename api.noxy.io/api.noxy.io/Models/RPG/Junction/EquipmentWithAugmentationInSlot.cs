using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.noxy.io.Models.RPG.Junction
{
    [Table("$EquipmentWithAugmentationInSlot")]
    [PrimaryKey(nameof(ItemEquipmentID), nameof(ItemAugmentationID), nameof(SlotAugmentationID))]
    public class EquipmentWithAugmentationInSlot
    {
        [Required]
        public required Guid ItemEquipmentID { get; set; }
        public required ItemEquipment ItemEquipment { get; set; }

        [Required]
        public required Guid ItemAugmentationID { get; set; }
        public required ItemAugmentation ItemAugmentation { get; set; }

        [Required]
        public required Guid SlotAugmentationID { get; set; }
        public required SlotAugmentation SlotAugmentation { get; set; }

        [Required]
        public DateTime TimeCreated { get; set; } = DateTime.UtcNow;
    }
}
