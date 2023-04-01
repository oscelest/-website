using api.noxy.io.Models.Game.Modifier;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace api.noxy.io.Models.Game.Trigger
{
    public class DamageReceivedTriggerEntity : TriggerEntity
    {
        public List<ModifierEntity> ModifierList { get; set; } = new();

        public DamageReceivedTriggerEntity() : base(TriggerEntityType.DamageReceived) { }

        new public DTO ToDTO() => new(this);

        public static void AddTableToBuilder(EntityTypeBuilder<DamageReceivedTriggerEntity> builder)
        {
            builder.HasMany(x => x.ModifierList).WithMany().UsingEntity(x => x.ToTable($"_{nameof(DamageReceivedTriggerEntity)}-{nameof(ModifierEntity)}"));
        }

        new public class DTO : SingleEntity.DTO
        {
            public IEnumerable<ModifierEntity.DTO> ModifierList { get; set; }

            public DTO(DamageReceivedTriggerEntity entity) : base(entity)
            {
                ModifierList = entity.ModifierList.Select(x => x.ToDTO());
            }
        }
    }
}
