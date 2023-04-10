using api.noxy.io.Models.RPG.Junction;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.noxy.io.Models.RPG
{
    [Table("Equipment")]
    [Table("ItemEquipment")]
    public class Equipment : GuildWithItem
    {
        [Required]
        public required Item.Equipment ItemType { get; set; }

        [Required]
        public DateTime TimeCreated { get; set; } = DateTime.UtcNow;

        // Mappings
        public required List<UnitWithEquipmentInSlot> UnitEquipmentSlotList { get; set; }
        public required List<EquipmentWithAugmentationInSlot> EquipmentAugmentationSlotList { get; set; }
    }
}
