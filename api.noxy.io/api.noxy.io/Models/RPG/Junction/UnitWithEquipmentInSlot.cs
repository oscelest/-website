using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.noxy.io.Models.RPG.Junction
{
    [Table("$UnitWithEquipmentInSlot")]
    [PrimaryKey(nameof(Unit), nameof(ItemEquipment), nameof(SlotEquipment))]
    public class UnitWithEquipmentInSlot
    {
        [Required]
        public required Unit Unit { get; set; }

        [Required]
        public required Item.Equipment ItemEquipment { get; set; }

        [Required] 
        public required Slot.Equipment SlotEquipment { get; set; }

        [Required]
        public DateTime TimeCreated { get; set; } = DateTime.UtcNow;

    }
}
