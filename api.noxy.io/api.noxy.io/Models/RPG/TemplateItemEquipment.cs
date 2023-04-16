using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.noxy.io.Models.RPG
{
    [Table("TemplateItemEquipment")]
    public class TemplateItemEquipment : TemplateItem
    {
        [Required]
        public required TemplateSlot TemplateSlot { get; set; }

        public List<TemplateSlot> TemplateSlotList { get; set; }

        #region -- Mappings --

        public List<ItemEquipment> ItemEquipmentList { get; set; } = new();

        #endregion -- Mappings --
    }
}
