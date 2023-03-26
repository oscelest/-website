using api.noxy.io.Utilities;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.noxy.io.Models.Game.Guild
{
    [Table("Statistic")]
    public class StatisticEntity : SimpleEntity
    {
        public float Value { get; set; } = 0f;
        public required StatisticTagType Tag { get; set; }

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
