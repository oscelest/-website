using api.noxy.io.Models.RPG.Junction;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.noxy.io.Models.RPG
{
    [Table("Unit")]
    public class Unit
    {
        [Key]
        public Guid ID { get; set; } = Guid.NewGuid();

        [Required]
        public required Race Race { get; set; }

        [Required]
        public required Guild Guild { get; set; }

        [Required]
        public required bool Initiated { get; set; }

        [Required]
        public required int Experience { get; set; }

        public DateTime? TimeInitiated { get; set; }

        [Required]
        public DateTime TimeCreated { get; set; } = DateTime.UtcNow;

        // Mappings
        public required List<UnitWithEquipmentInSlot> UnitEquipmentSlotList { get; set; }
    }
}
