using System.ComponentModel.DataAnnotations;

namespace Database.Models.RPG.Abstract
{
    public abstract class Template
    {
        [Key]
        public Guid ID { get; set; } = Guid.NewGuid();

        [Required]
        public required string Name { get; set; }

        [Required]
        public DateTime TimeCreated { get; set; } = DateTime.UtcNow;
    }
}
