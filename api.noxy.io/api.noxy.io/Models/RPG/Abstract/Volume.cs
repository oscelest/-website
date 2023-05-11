using System.ComponentModel.DataAnnotations;

namespace Database.Models.RPG.Abstract
{
    public abstract class Volume
    {
        [Key]
        public Guid ID { get; set; } = Guid.NewGuid();

        [Required]
        public required int Count { get; set; }
    }
}
