using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.noxy.io.Models.Game.Guild
{
    [Table("Mission")]
    public class MissionEntity : SingleEntity
    {
        [Required]
        public required int BaseDuration { get; set; }

        public UnitEntity? Unit { get; set; }
        public Guid? UnitID { get; set; }

        public required GuildEntity Guild { get; set; }
        public required Guid GuildID { get; set; }

        public DateTime? TimeStarted { get; set; }

        #region -- DTO --

        new public DTO ToDTO() => new(this);

        new public class DTO : SingleEntity.DTO
        {
            public int BaseDuration { get; set; }
            public UnitEntity.DTO? Unit { get; set; }
            public DateTime? TimeStarted { get; set; }

            public DTO(MissionEntity entity) : base(entity)
            {
                BaseDuration = entity.BaseDuration;
                Unit = entity.Unit?.ToDTO();
                TimeStarted = entity.TimeStarted;
            }
        }

        #endregion -- DTO --

    }
}
