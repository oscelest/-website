using Database.Models.RPG.Junction;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.noxy.io.Models.RPG
{
    [Table("TemplateItemGear")]
    public class TemplateItemGear : TemplateItem
    {
        public List<SlotSupport> SlotSupportList { get; set; } = new();

        public List<TemplateItemGearWithTemplateSlot> TemplateItemGearWithSlotGearList { get; set; } = new();
    }
}
