using System.ComponentModel.DataAnnotations;

namespace api.noxy.io.Models.RPG
{
    public class EquipmentGear : Equipment
    {
        [Required]
        public required TemplateItemGear TemplateItemGear { get; set; }

        public required Unit? Unit { get; set; }

        public List<SlotGear> SlotGearList { get; set; } = new();
    }
}
