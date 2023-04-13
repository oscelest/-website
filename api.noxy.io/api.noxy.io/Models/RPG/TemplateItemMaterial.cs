using System.ComponentModel.DataAnnotations.Schema;

namespace api.noxy.io.Models.RPG
{
    [Table("TemplateItemMaterial")]
    public class TemplateItemMaterial : TemplateItem
    {
        public required List<TemplateSlot> TemplateSlotList { get; set; }
    }
}
