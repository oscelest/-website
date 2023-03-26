using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.noxy.io.Models.Game.Guild
{
    [Table("Role")]
    [Index(nameof(Name), IsUnique = true)]
    public class RoleEntity : SimpleEntity
    {
        [Required]
        [MinLength(3), MaxLength(64)]
        public string Name { get; set; } = string.Empty;

        // Mappings
        public RoleTypeEntity RoleType { get; set; } = new();
        public List<FeatEntity> FeatList { get; set; } = new();

        // Inverse
        public List<RoleLevelEntity> RoleLevelList { get; set; } = new();

        #region -- DTO --

        new public DTO ToDTO() => new(this);

        new public class DTO : SimpleEntity.DTO
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
