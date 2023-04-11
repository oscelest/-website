using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.noxy.io.Models.RPG
{
    public abstract class Item
    {
        [Key]
        public Guid ID { get; set; }

        [Required]
        public required Guild Guild { get; set; }

        [Required]
        public required Template.Item TemplateItemRef { get; set; }

        [Required]
        public DateTime TimeCreated { get; set; }

        // Implementations

        [Table("ItemEquipment")]
        public class Equipment : Item
        {

        }

        [Table("ItemAugmentation")]
        public class Augmentation : Item
        {

        }

        [Table("ItemMaterial")]
        public class Material : Item
        {
            public int Count { get; set; }
        }
    }
}
