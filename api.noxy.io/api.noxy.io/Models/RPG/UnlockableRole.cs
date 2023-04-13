using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.noxy.io.Models.RPG
{
    [Table("UnlockableRole")]
    public class UnlockableRole : Unlockable
    {
        [Required]
        public required TemplateRole TemplateRole { get; set; }
    }
}
