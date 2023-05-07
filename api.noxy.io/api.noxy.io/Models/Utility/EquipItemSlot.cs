using api.noxy.io.Models.RPG;
using Database.Models.RPG.Junction;

namespace Database.Models.Utility
{
    public class EquipItemSlot
    {
        public required TemplateItemGearUseTemplateSlot TemplateItemGearUseTemplateSlot { get; set; }
        public required SlotGear? SlotGear { get; set; }
    }
}
