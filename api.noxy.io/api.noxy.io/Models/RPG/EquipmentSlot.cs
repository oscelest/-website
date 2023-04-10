using api.noxy.io.Models.RPG.Junction;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.noxy.io.Models.RPG
{
    [Table("EquipmentSlot")]
    public class EquipmentSlot
    {
        [Key]
        public Guid ID { get; set; } = Guid.NewGuid();

        [Required]
        public required Race Race { get; set; }

        [Required]
        public required Slot Slot { get; set; }

        // Mappings
        public required List<UnitWithEquipmentInSlot> UnitEquipmentSlotList { get; set; }
    }
}
