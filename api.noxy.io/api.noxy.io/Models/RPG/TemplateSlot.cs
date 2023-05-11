using Database.Models.RPG.Abstract;
using Database.Models.RPG.Junction;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Models.RPG
{
    [Table("TemplateSlot")]
    public class TemplateSlot : Template
    {
        #region -- Mappings --

        public List<SlotGear> SlotGearList { get; set; } = new();

        public List<TemplateItemGearUseTemplateSlot> TemplateItemGearWithTemplateSlotList { get; set; } = new();

        public List<TemplateItemSupportUseTemplateSlot> TemplateItemSupportUseTemplateSlotList { get; set; } = new();

        #endregion -- Mappings --
    }
}
