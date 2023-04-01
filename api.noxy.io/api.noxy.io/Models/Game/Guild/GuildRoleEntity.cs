using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.noxy.io.Models.Game.Guild
{
    [Table("GuildRole")]
    public class GuildRoleEntity : JunctionEntity
    {
        [Required]
        public required RoleEntity Role { get; set; }

        [Required]
        public required GuildEntity Guild { get; set; }
    }
}
