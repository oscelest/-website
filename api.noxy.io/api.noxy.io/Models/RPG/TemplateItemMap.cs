using Database.Models.RPG.Abstract;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Models.RPG
{
    [Table("TemplateItemMap")]
    public class TemplateItemMap : TemplateItem
    {
        public required List<SlotSupport> SlotSupportList { get; set; }
    }
}
