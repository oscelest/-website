using api.noxy.io.Utilities;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.noxy.io.Models.Game.Guild
{
    [Table("MissionStatistic")]
    public class MissionStatisticEntity : StatisticEntity
    {
        public MissionTypeEntity MissionType { get; set; } = new();

        #region -- DTO --

        new public DTO ToDTO() => new(this);

        new public class DTO : SimpleEntity.DTO
        {
            public float Value { get; set; }
            public StatisticTagType Tag { get; set; }

            public DTO(StatisticEntity entity) : base(entity)
            {
                Value = entity.Value;
                Tag = entity.Tag;
            }
        }

        #endregion -- DTO --

    }
}
