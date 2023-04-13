using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.noxy.io.Models.RPG
{
    [Table("UnlockableRecipe")]
    public class UnlockableRecipe : Unlockable
    {
        [Required]
        public required TemplateRecipe TemplateRecipe { get; set; }
    }
}
