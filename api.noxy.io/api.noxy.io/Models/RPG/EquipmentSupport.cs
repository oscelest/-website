using System.ComponentModel.DataAnnotations;

namespace api.noxy.io.Models.RPG
{
    public class EquipmentSupport : Equipment
    {
        [Required]
        public required SlotSupport SlotSupport { get; set; }

        [Required]
        public required TemplateItemSupport TemplateItemSupport { get; set; }

        [Required]
        public required EquipmentGear EquipmentGear { get; set; }
    }
}
