using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.noxy.io.Models.RPG
{
    [Table("TemplateRecipe")]
    public class TemplateRecipe : Template
    {
        [Comment("A list of items required for performing the recipe.")]
        public required List<VolumeItem> InputVolumeItemList { get; set; }

        [Comment("A list of items rewarded for performing the recipe.")]
        public required List<VolumeItem> OutputVolumeItemList { get; set; }
    }
}
