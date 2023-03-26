using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.noxy.io.Models.Game.Guild
{
    [Table("RoleLevel")]
    public class RoleLevelEntity : SimpleEntity
    {
        public int Experience { get; set; } = 0;

        // Mappings
        public UnitEntity Unit { get; set; } = new();
        public RoleEntity Role { get; set; } = new();

        #region -- DTO --

        new public DTO ToDTO() => new(this);

        new public class DTO : SimpleEntity.DTO
        {
            public int Experience { get; set; }
            public RoleEntity.DTO Role { get; set; }

            public DTO(RoleLevelEntity entity) : base(entity)
            {
                Experience = entity.Experience;
                Role = entity.Role.ToDTO();
            }
        }

        #endregion -- DTO --

    }
}
