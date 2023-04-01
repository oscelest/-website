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
        public required RoleTypeEntity RoleType { get; set; }

        #region -- Mapping --

        public List<RequirementEntity> RequirementList { get; set; } = new();

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
