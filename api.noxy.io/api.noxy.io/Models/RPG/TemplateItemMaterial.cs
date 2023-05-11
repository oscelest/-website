using Database.Models.RPG.Abstract;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Models.RPG
{
    [Table("TemplateItemMaterial")]
    public class TemplateItemMaterial : TemplateItem
    {
        public required string Description { get; set; }
    }
}
