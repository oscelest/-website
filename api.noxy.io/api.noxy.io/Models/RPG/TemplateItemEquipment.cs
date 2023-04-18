using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.noxy.io.Models.RPG
{
    [Table("TemplateItemEquipment")]
    public class TemplateItemEquipment : TemplateItem
    {
        [Required]
        public required TemplateSlot TemplateSlot { get; set; }

        public List<TemplateSlot> TemplateSlotList { get; set; } = new();
    }
}
