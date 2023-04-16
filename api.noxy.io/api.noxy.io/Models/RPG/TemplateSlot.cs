using System.ComponentModel.DataAnnotations.Schema;

namespace api.noxy.io.Models.RPG
{
    [Table("TemplateSlot")]
    public class TemplateSlot : Template
    {
        #region -- Mappings --

        public List<SlotAugmentation> SlotAugmentationList { get; set; } = new();
        public List<SlotEquipment> SlotEquipmentList { get; set; } = new();

        #endregion -- Mappings --
    }
}
