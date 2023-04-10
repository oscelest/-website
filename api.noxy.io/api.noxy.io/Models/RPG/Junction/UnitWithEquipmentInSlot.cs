using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.noxy.io.Models.RPG.Junction
{
    [Table("$UnitWithEquipmentInSlot")]
    [PrimaryKey(nameof(Unit), nameof(Equipment), nameof(EquipmentSlot))]
    public class UnitWithEquipmentInSlot
    {
        [Required]
        public required Unit Unit { get; set; }

        [Required]
        public required GuildWithItem Equipment { get; set; }

        [Required] 
        public required EquipmentSlot EquipmentSlot { get; set; }

        [Required]
        public DateTime TimeCreated { get; set; } = DateTime.UtcNow;

    }
}
