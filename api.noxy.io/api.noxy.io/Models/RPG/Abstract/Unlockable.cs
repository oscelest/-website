using System.ComponentModel.DataAnnotations;

namespace Database.Models.RPG.Abstract
{
    public abstract class Unlockable
    {
        [Key]
        public Guid ID { get; set; } = Guid.NewGuid();

        [Required]
        public required Guild Guild { get; set; }

        [Required]
        public DateTime TimeAcquired { get; set; } = DateTime.UtcNow;
    }
}
