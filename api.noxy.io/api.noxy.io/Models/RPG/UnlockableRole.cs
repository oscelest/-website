using Database.Models.RPG.Abstract;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Models.RPG
{
    [Table("UnlockableRole")]
    public class UnlockableRole : Unlockable
    {
        [Required]
        public required TemplateRole TemplateRole { get; set; }
    }
}
