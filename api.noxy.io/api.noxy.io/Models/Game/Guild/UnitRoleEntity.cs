using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.noxy.io.Models.Game.Guild
{
    [Table("UnitRole")]
    public class UnitRoleEntity : SingleEntity
    {
        [Required]
        [Comment("The amount of experience the unit has with the owned role.")]
        public int Experience { get; set; } = 0;

        [Required]
        [Comment("The unit owning the role")]
        public required UnitEntity Unit { get; set; }

        [Required]
        [Comment("The role being owning by the unit")]
        public required GuildRoleEntity Role { get; set; } 

        #region -- DTO --

        new public DTO ToDTO() => new(this);

        new public class DTO : SingleEntity.DTO
        {
            public int Experience { get; set; }
            public GuildRoleEntity.DTO Role { get; set; }

            public DTO(UnitRoleEntity entity) : base(entity)
            {
                Experience = entity.Experience;
                Role = entity.Role.ToDTO();
            }
        }

        #endregion -- DTO --

    }
}
