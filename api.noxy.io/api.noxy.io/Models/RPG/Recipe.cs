using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.noxy.io.Models.RPG
{
    [Table("RecipeType")]
    public class Recipe
    {
        [Key]
        public Guid ID { get; set; } = Guid.NewGuid();

        [Comment("A list of items required for performing the recipe.")]
        public required List<ItemVolume> ListNeededItemVolume { get; set; }

        [Comment("A list of items rewarded for performing the recipe.")]
        public required List<ItemVolume> ListResultItemVolume { get; set; }

        [Required]
        public DateTime TimeCreated { get; set; } = DateTime.UtcNow;
    }
}
