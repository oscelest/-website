using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.noxy.io.Models.RPG
{
    public abstract class Unlockable
    {
        [Required]
        public required Guild Guild { get; set; }

        [Required]
        public required DateTime TimeAcquired { get; set; }

        // Implementations

        [Table("UnlockableRole")]
        [PrimaryKey(nameof(Guild), nameof(TemplateRole))]
        public class Role : Unlockable
        {
            [Required]
            public required Template.Role TemplateRole { get; set; }
        }


        [Table("UnlockableFeat")]
        [PrimaryKey(nameof(Guild), nameof(TemplateFeat))]
        public class Feat : Unlockable
        {
            [Required]
            public required Template.Feat TemplateFeat { get; set; }
        }
    }
}
