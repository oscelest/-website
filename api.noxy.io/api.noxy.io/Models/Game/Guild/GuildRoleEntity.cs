using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using api.noxy.io.Models.Game.Role;

namespace api.noxy.io.Models.Game.Guild
{
    [Table("Role")]
    public class GuildRoleEntity : JunctionEntity
    {
        [Required]
        public required GuildEntity Guild { get; set; }

        [Required]
        public required RoleEntity Role { get; set; }


    }
}
