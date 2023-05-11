using Database.Models.RPG.Abstract;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Models.RPG
{
    [Table("TemplateRecipe")]
    public class TemplateRecipe : Template
    {
        [Comment("A list of items required for performing the recipe.")]
        public List<VolumeItemRecipe> VolumeItemRecipeList { get; set; } = new();

        #region -- Mappings --

        public List<UnlockableRecipe> UnlockableRecipeList { get; set; } = new();

        #endregion -- Mappings --
    }
}
