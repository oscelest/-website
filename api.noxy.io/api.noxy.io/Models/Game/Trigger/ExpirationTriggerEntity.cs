using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace api.noxy.io.Models.Game.Trigger
{
    public class ExpirationTriggerEntity : TriggerEntity
    {
        public EffectExpirationType ExpirationType { get; set; }

        public ExpirationTriggerEntity() : base(TriggerEntityType.Expiration) { }

        new public DTO ToDTO() => new(this);

        public static void AddTableToBuilder(EntityTypeBuilder<ExpirationTriggerEntity> builder)
        {
            builder.Property(x => x.ExpirationType);
        }

        new public class DTO : SimpleEntity.DTO
        {
            public string ExpirationType { get; set; }

            public DTO(ExpirationTriggerEntity entity) : base(entity)
            {
                ExpirationType = entity.ExpirationType.ToString();
            }
        }
    }

    public enum EffectExpirationType
    {
        Duration,
        Cleanse,
        Dispel,
        Death
    }
}
