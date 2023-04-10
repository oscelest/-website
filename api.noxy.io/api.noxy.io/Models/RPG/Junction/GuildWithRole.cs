using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.noxy.io.Models.RPG.Junction
{
    [Table("GuildWithRecipe")]
    [PrimaryKey(nameof(Guild), nameof(Role))]
    public class GuildWithRole
    {
        [Required]
        public required Guild Guild { get; set; }

        [Required]
        public required Role Role { get; set; }

        [Required]
        public required DateTime TimeAcquired { get; set; }
    }
}
