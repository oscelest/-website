using System.ComponentModel.DataAnnotations;
using Database.Models.RPG.Abstract;

namespace Database.Models.RPG
{
    public class Item
    {
        [Key]
        public Guid ID { get; set; } = Guid.NewGuid();

        [Required]
        public required Guild Guild { get; set; }

        [Required]
        public required int Count { get; set; }

        [Required]
        public required TemplateItem TemplateItem { get; set; }

        [Required]
        public DateTime TimeCreated { get; set; } = DateTime.UtcNow;

        public TemplateItemGear TemplateItemGear { get => (TemplateItemGear)TemplateItem; }

        public TemplateItemSupport TemplateItemSupport { get => (TemplateItemSupport)TemplateItem; }
    }
}
