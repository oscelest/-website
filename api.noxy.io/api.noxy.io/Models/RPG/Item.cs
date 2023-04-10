using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace api.noxy.io.Models.RPG
{
    public abstract class Item
    {
        [Key]
        public Guid ID { get; set; } = Guid.NewGuid();

        [Required]
        public required string Name { get; set; }



        [Table("ItemAugmentation")]
        public class Augmentation : Item
        {
            [Required]
            public required Slot Slot { get; set; }

            public required List<Guid> ModifierList { get; set; }

            [Required]
            public DateTime TimeCreated { get; set; } = DateTime.UtcNow;
        }



        [Table("ItemEquipment")]
        public class Equipment : Item
        {
            [Required]
            public required Slot Slot { get; set; }

            public required List<AugmentationSlot> AugmentationSlotList { get; set; }

            [Required]
            public DateTime TimeCreated { get; set; } = DateTime.UtcNow;
        }



        [Table("ItemMap")]
        public class Map : Item
        {
            public required List<AugmentationSlot> AugmentationSlotList { get; set; }

        }
    }
}
