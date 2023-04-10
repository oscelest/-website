using api.noxy.io.Models.Game.Guild;
using api.noxy.io.Models.Game.Mission;
using api.noxy.io.Models.Game.Requirement;
using api.noxy.io.Models.Game.Unit;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.noxy.io.Models.Game.Role
{
    [Table("Role")]
    [Index(nameof(Name), IsUnique = true)]
    public class RoleEntity : SingleEntity
    {
        [Required]
        [MinLength(3), MaxLength(64)]
        public required string Name { get; set; }

        [Required]
        public required RoleTypeEntity RoleType { get; set; }

        [Comment("The list of requirements for a guild to unlock this role")]
        public List<RequirementEntity> RequirementList { get; set; } = new();

        #region -- Mapping --

        public List<MissionEntity> MissionList { get; set; } = new();
        public List<UnitRoleEntity> UnitRoleList { get; set; } = new();
        public List<GuildRoleEntity> GuildRoleList { get; set; } = new();

        #endregion -- Mapping --

        #region -- DTO --

        new public DTO ToDTO() => new(this);

        new public class DTO : SingleEntity.DTO
        {
            public string Name { get; set; }
            public RoleTypeEntity.DTO RoleType { get; set; }

            public DTO(RoleEntity entity) : base(entity)
            {
                Name = entity.Name;
                RoleType = entity.RoleType.ToDTO();
            }
        }

        #endregion -- DTO --

    }
}
