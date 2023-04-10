using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.noxy.io.Models.RPG.Junction
{
    [Table("$EquipmentWithAugmentationInSlot")]
    [PrimaryKey(nameof(Equipment), nameof(Augmentation), nameof(AugmentationSlot))]
    public class EquipmentWithAugmentationInSlot
    {
        [Required]
        public required GuildWithItem Equipment { get; set; }

        [Required]
        public required GuildWithItem Augmentation { get; set; }

        [Required]
        public required AugmentationSlot AugmentationSlot { get; set; }

        [Required]
        public DateTime TimeCreated { get; set; } = DateTime.UtcNow;
    }
}
