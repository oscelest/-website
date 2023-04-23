using System.ComponentModel.DataAnnotations;

namespace api.noxy.io.Models.RPG
{
    public abstract class Equipment
    {
        [Key]
        public Guid ID { get; set; } = Guid.NewGuid();

        [Required]
        public DateTime TimeCreated { get; set; } = DateTime.UtcNow;
    }
}
