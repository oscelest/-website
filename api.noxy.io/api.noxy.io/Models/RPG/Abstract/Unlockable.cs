using System.ComponentModel.DataAnnotations;

namespace api.noxy.io.Models.RPG
{
    public abstract class Unlockable
    {
        [Key]
        public Guid ID { get; set; } = Guid.NewGuid();

        [Required]
        public required Guild Guild { get; set; }

        [Required]
        public required DateTime TimeAcquired { get; set; }
    }
}
