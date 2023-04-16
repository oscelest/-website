using System.ComponentModel.DataAnnotations;

namespace api.noxy.io.Models.RPG
{
    public class VolumeItemRecipe : VolumeItem
    {
        [Required]
        public required TemplateRecipe TemplateRecipe { get; set; }
        
        [Required]
        public required bool Input { get; set; }
    }
}
