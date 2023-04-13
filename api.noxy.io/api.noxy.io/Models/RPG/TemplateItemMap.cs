using System.ComponentModel.DataAnnotations.Schema;

namespace api.noxy.io.Models.RPG
{
    [Table("TemplateItemMap")]
    public class TemplateItemMap : TemplateItem
    {
        public required List<TemplateSlot> TemplateSlotList { get; set; }
    }
}
