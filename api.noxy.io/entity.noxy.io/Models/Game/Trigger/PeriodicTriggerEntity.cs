using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace api.noxy.io.Models.Game.Trigger
{
    public class PeriodicTriggerEntity : TriggerEntity
    {
        public int Interval { get; set; } = 1000;

        public PeriodicTriggerEntity() : base(TriggerEntityType.Periodic) { }

        new public DTO ToDTO() => new(this);

        public static void AddTableToBuilder(EntityTypeBuilder<PeriodicTriggerEntity> builder)
        {
            builder.Property(x => x.Interval);
        }

        new public class DTO : SingleEntity.DTO
        {
            public int Interval { get; set; }

            public DTO(PeriodicTriggerEntity entity) : base(entity)
            {
                Interval = entity.Interval;
            }
        }
    }
}
