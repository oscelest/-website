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
        public class Role
        {
            [Required]
            public required Template.Role TemplateRole { get; set; }
        }
    }
}
