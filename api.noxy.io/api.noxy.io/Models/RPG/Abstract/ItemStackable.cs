using System.ComponentModel.DataAnnotations;

namespace api.noxy.io.Models.RPG
{
    public abstract class ItemStackable : Item
    {
        [Required]
        public int Count { get; set; }
    }
}
