using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using api.noxy.io.Exceptions;
using api.noxy.io.Models.RPG;

namespace Database.Models.RPG.Junction
{
    [Table("$TemplateItemGear-TemplateSlot")]
    public class TemplateItemGearUseTemplateSlot
    {
        [Key]
        public Guid ID { get; set; } = Guid.NewGuid();

        public required TemplateItemGear TemplateItemGear { get; set; }

        public required TemplateSlot TemplateSlot { get; set; }

        [Required]
        public DateTime TimeCreated { get; set; } = DateTime.UtcNow;
    }
}
