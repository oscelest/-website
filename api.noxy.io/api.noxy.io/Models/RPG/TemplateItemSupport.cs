using Database.Models.RPG.Abstract;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Models.RPG
{
    [Table("TemplateItemSupport")]
    public class TemplateItemSupport : TemplateItem
    {
        [Required]
        public List<TemplateSlot> TemplateSlot { get; set; } = new();

        public List<ModifierItem> ModifierItemList { get; set; } = new();
    }
}
