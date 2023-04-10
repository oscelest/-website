using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.noxy.io.Models.RPG.Junction
{
    [Table("GuildWithItem")]
    [PrimaryKey(nameof(Guild), nameof(Item))]
    public abstract class GuildWithItem
    {
        [Required]
        public required Guild Guild { get; set; }

        [Required]
        public required Item Item { get; set; }

        [Required]
        public DateTime TimeCreated { get; set; }
    }
}
