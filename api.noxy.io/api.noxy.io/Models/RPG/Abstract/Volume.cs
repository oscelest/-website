using System.ComponentModel.DataAnnotations;

namespace api.noxy.io.Models.RPG
{
    public abstract class Volume
    {
        [Key]
        public Guid ID { get; set; } = Guid.NewGuid();

        [Required]
        public required int Count { get; set; }
    }
}
