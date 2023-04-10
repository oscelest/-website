using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.noxy.io.Models.RPG.Junction
{
    [Table("GuildWithRecipe")]
    [PrimaryKey(nameof(Guild), nameof(Recipe))]
    public class GuildWithRecipe
    {
        [Required]
        public required Guild Guild { get; set; }

        [Required]
        public required Recipe Recipe { get; set; }

        [Required]
        public required DateTime TimeCreated { get; set; }
    }
}
