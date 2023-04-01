using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.noxy.io.Models.Game.Guild
{
    [Table("RoleLevel")]
    public class RoleLevelEntity : SingleEntity
    {
        [Required]
        public int Experience { get; set; } = 0;

        public required UnitEntity Unit { get; set; }
        public required Guid UnitID { get; set; }

        [Required]
        public required RoleEntity Role { get; set; } 
        public required Guid RoleID { get; set; } 

        #region -- DTO --

        new public DTO ToDTO() => new(this);

        new public class DTO : SingleEntity.DTO
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
