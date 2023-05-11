using Database.Models.RPG.Abstract;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Models.RPG
{
    [Table("UnlockableRecipe")]
    public class UnlockableRecipe : Unlockable
    {
        [Required]
        public required TemplateRecipe TemplateRecipe { get; set; }
    }
}
