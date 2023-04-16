using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.noxy.io.Models.RPG
{
    [Table("TemplateRecipe")]
    public class TemplateRecipe : Template
    {
        [Comment("A list of items required for performing the recipe.")]
        public required List<VolumeItemRecipe> VolumeItemRecipeList { get; set; }

        #region -- Mappings --

        public List<UnlockableRecipe> UnlockableRecipeList { get; set; } = new();

        #endregion -- Mappings --
    }
}
