using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.noxy.io.Models.Game.Guild
{
    [Table("Role")]
    [Index(nameof(Name), IsUnique = true)]
    public class RoleEntity : SingleEntity
    {
        [Required]
        [MinLength(3), MaxLength(64)]
        public required string Name { get; set; }

        [Required]
        public required RoleTypeEntity RoleGroup { get; set; }

        [Comment("The list of requirements for a guild to unlock this role")]
        public List<RequirementEntity> RequirementList { get; set; } = new();

        #region -- Mapping --

        public List<MissionEntity> MissionList { get; set; } = new();
        public List<UnitRoleEntity> UnitRoleList { get; set; } = new();

        #endregion -- Mapping --

        #region -- DTO --

        new public DTO ToDTO() => new(this);

        new public class DTO : SingleEntity.DTO
        {
            public string Name { get; set; }
            public RoleTypeEntity.DTO RoleGroup { get; set; }

            public DTO(RoleEntity entity) : base(entity)
            {
                Name = entity.Name;
                RoleGroup = entity.RoleGroup.ToDTO();
            }
        }

        #endregion -- DTO --

    }
}
