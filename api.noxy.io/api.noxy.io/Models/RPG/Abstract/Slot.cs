using System.ComponentModel.DataAnnotations;

namespace api.noxy.io.Models.RPG
{
    public abstract class Slot
    {
        [Key]
        public Guid ID { get; set; } = Guid.NewGuid();

        [Required]
        public required TemplateSlot TemplateSlot { get; set; }

        [Required]
        public DateTime TimeCreated { get; set; } = DateTime.UtcNow;
    }
}
