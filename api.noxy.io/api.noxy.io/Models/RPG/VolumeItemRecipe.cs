using Database.Models.RPG.Abstract;
using System.ComponentModel.DataAnnotations;

namespace Database.Models.RPG
{
    public class VolumeItemRecipe : VolumeItem
    {
        [Required]
        public required TemplateRecipe TemplateRecipe { get; set; }

        [Required]
        public required bool Component { get; set; }
    }
}
