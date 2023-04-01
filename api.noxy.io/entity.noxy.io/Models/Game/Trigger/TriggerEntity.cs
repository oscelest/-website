using api.noxy.io.Models.Game.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace api.noxy.io.Models.Game.Trigger
{
    public abstract class TriggerEntity : SingleEntity
    {
        public TriggerEntityType EntityType { get; set; }
        public List<OperationEntity> OperationList { get; set; } = new();

        protected TriggerEntity(TriggerEntityType type)
        {
            EntityType = type;
        }

        new public DTO ToDTO() => new(this);

        public static void AddTableToBuilder(EntityTypeBuilder<TriggerEntity> builder)
        {
            builder.ToTable(nameof(TriggerEntity));
            builder.HasMany(x => x.OperationList).WithMany().UsingEntity(x => x.ToTable($"_{nameof(TriggerEntity)}-{nameof(OperationEntity)}"));
            builder.HasDiscriminator(x => x.EntityType)
            .HasValue<DamageReceivedTriggerEntity>(TriggerEntityType.DamageReceived)
            .HasValue<ExpirationTriggerEntity>(TriggerEntityType.Expiration)
            .HasValue<HealingReceivedTriggerEntity>(TriggerEntityType.HealingReceived)
            .HasValue<PeriodicTriggerEntity>(TriggerEntityType.Periodic);
        }

        new public class DTO : SingleEntity.DTO
        {
            public TriggerEntityType EntityType { get; set; }
            public IEnumerable<OperationEntity.DTO> OperationList { get; set; }

            public DTO(TriggerEntity entity) : base(entity)
            {
                EntityType = entity.EntityType;
                OperationList = entity.OperationList.Select(x => x.ToDTO());
            }
        }
    }

    public enum TriggerEntityType
    {
        DamageReceived = 0,
        Expiration = 1,
        HealingReceived = 2,
        Periodic = 3,
    }
}
