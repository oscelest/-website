using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.noxy.io.Models.Game.Guild
{
    [Table("Mission")]
    public class MissionEntity : SingleEntity
    {
        [Required]
        public required int BaseDuration { get; set; }

        [Required]
        public required GuildEntity Guild { get; set; }

        public DateTime? TimeStarted { get; set; } = default;

        #region -- Mapping --

        public List<UnitEntity> UnitList { get; set; } = new();

        #endregion -- Mapping --

        #region -- DTO --

        new public DTO ToDTO() => new(this);

        new public class DTO : SingleEntity.DTO
        {
            public int BaseDuration { get; set; }
            public IEnumerable<UnitEntity.DTO> UnitList { get; set; }
            public DateTime? TimeStarted { get; set; }

            public DTO(MissionEntity entity) : base(entity)
            {
                BaseDuration = entity.BaseDuration;
                UnitList = entity.UnitList.Select(x => x.ToDTO());
                TimeStarted = entity.TimeStarted;
            }
        }

        #endregion -- DTO --

    }
}
