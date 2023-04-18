using System.ComponentModel.DataAnnotations;

namespace api.noxy.io.Models.RPG
{
    public abstract class Item
    {
        [Key]
        public Guid ID { get; set; } = Guid.NewGuid();

        [Required]
        public required Guild Guild { get; set; }

        [Required]
        public required TemplateItem TemplateItem { get; set; }

        [Required]
        public DateTime TimeCreated { get; set; } = DateTime.UtcNow;
    }
}
