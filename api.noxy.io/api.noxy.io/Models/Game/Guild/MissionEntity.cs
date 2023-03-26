using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.noxy.io.Models.Game.Guild
{
    [Table("Mission")]
    public class MissionEntity : SimpleEntity
    {
        [Required]
        public int BaseDuration { get; set; } = 0;

        public UnitEntity? Unit { get; set; }

        public DateTime? TimeStarted { get; set; }

        // Mapping
        public ICollection<RoleEntity> RoleList { get; set; } = new HashSet<RoleEntity>();

        #region -- DTO --

        new public DTO ToDTO() => new(this);

        new public class DTO : SimpleEntity.DTO
        {
            public int BaseDuration { get; set; }
            public UnitEntity? Unit { get; set; }
            public DateTime? TimeStarted { get; set; }

            public DTO(MissionEntity entity) : base(entity)
            {
                BaseDuration = entity.BaseDuration;
                Unit = entity.Unit;
                TimeStarted = entity.TimeStarted;
            }
        }

        #endregion -- DTO --

    }
}
