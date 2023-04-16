using System.ComponentModel.DataAnnotations.Schema;

namespace api.noxy.io.Models.RPG
{
    [Table("TemplateItemMap")]
    public class TemplateItemMap : TemplateItem
    {
        public required List<TemplateSlot> TemplateSlotList { get; set; }

        #region -- Mappings --

        public List<ItemMap> ItemMapList { get; set; } = new();

        #endregion -- Mappings --
    }
}
