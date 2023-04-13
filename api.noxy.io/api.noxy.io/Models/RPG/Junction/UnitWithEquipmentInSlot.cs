using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.noxy.io.Models.RPG.Junction
{
    [Table("$UnitWithEquipmentInSlot")]
    [PrimaryKey(nameof(UnitID), nameof(ItemEquipmentID), nameof(SlotEquipmentID))]
    public class UnitWithEquipmentInSlot
    {
        [Required]
        public Guid UnitID { get; set; }
        public required Unit Unit { get; set; }

        [Required]
        public Guid ItemEquipmentID { get; set; }
        public required ItemEquipment ItemEquipment { get; set; }

        [Required] 
        public Guid SlotEquipmentID { get; set; }
        public required SlotEquipment SlotEquipment { get; set; }

        [Required]
        public DateTime TimeCreated { get; set; } = DateTime.UtcNow;

    }
}
