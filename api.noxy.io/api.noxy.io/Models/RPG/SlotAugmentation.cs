using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.noxy.io.Models.RPG
{
    [Table("SlotAugmentation")]
    public class SlotAugmentation : Slot
    {
        [Required]
        public required TemplateItem TemplateItem { get; set; }
    }
}
