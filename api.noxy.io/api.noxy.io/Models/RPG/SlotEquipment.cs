using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.noxy.io.Models.RPG
{
    [Table("SlotEquipment")]
    public class SlotEquipment : Slot
    {
        [Required]
        public required TemplateUnit TemplateUnit { get; set; }
    }
}
