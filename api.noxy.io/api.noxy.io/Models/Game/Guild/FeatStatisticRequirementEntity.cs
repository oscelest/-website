using System.ComponentModel.DataAnnotations.Schema;

namespace api.noxy.io.Models.Game.Guild
{
    [Table("FeatStatisticRequirement")]
    public class FeatStatisticRequirementEntity : FeatRequirementEntity
    {
        public float Value { get; set; }
        public required StatisticEntity Statistic { get; set; }

        #region -- DTO --

        new public DTO ToDTO() => new(this);

        new public class DTO : SimpleEntity.DTO
        {
            public float Value { get; set; }
            public StatisticEntity Statistic { get; set; }

            public DTO(FeatStatisticRequirementEntity entity) : base(entity)
            {
                Value = entity.Value;
                Statistic = entity.Statistic;
            }
        }

        #endregion -- DTO --

    }
}
