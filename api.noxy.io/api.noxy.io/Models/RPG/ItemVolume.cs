using System.ComponentModel.DataAnnotations;

namespace api.noxy.io.Models.RPG
{
    public class ItemVolume
    {
        [Key]
        public Guid ID { get; set; } = Guid.NewGuid();

        [Required]
        public required Item Item { get; set; }

        [Required]
        public required int Count { get; set; }
    }
}
