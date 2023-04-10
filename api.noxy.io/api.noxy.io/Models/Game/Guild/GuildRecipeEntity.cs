using api.noxy.io.Models.Game.Recipe;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.noxy.io.Models.Game.Guild
{
    [Table("GuildRecipe")]
    public class GuildRecipeEntity : JunctionEntity
    {
        [Required]
        public required GuildEntity Guild { get; set; }

        [Required]
        public required RecipeEntity Recipe { get; set; }
    }
}
