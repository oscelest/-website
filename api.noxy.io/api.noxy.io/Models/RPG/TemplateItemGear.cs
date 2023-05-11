using Database.Models.RPG.Abstract;
using Database.Models.RPG.Junction;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Models.RPG
{
    [Table("TemplateItemGear")]
    public class TemplateItemGear : TemplateItem
    {
        public List<SlotSupport> SlotSupportList { get; set; } = new();

        public List<TemplateItemGearUseTemplateSlot> TemplateItemGearWithSlotGearList { get; set; } = new();

        public List<Modifier> ModifierList { get; set; } = new();
    }
}
