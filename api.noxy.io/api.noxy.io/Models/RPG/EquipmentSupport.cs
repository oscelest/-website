using Database.Models.RPG.Abstract;
using System.ComponentModel.DataAnnotations;

namespace Database.Models.RPG
{
    public class EquipmentSupport : Equipment
    {
        [Required]
        public required TemplateItemSupport TemplateItemSupport { get; set; }

        [Required]
        public required EquipmentGear EquipmentGear { get; set; }

        public List<SlotSupport> SlotSupportList { get; set; } = new();
    }
}
