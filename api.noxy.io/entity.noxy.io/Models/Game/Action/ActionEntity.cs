using api.noxy.io.Models.Game.Modifier;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace api.noxy.io.Models.Game.Action
{
    public abstract class ActionEntity : SingleEntity
    {
        public ActionEntityType EntityType { get; set; }
        public bool Periodic { get; set; } = new();
        public List<ModifierEntity> ModifierList { get; set; } = new();

        protected ActionEntity(ActionEntityType type)
        {
            EntityType = type;
        }

        new public DTO ToDTO() => new(this);

        public static void AddTableToBuilder(EntityTypeBuilder<ActionEntity> builder)
        {
            builder.ToTable(nameof(ActionEntity));
            builder.Property(e => e.Periodic).IsRequired().HasDefaultValue(false);
            builder.HasMany(x => x.ModifierList).WithMany().UsingEntity(x => x.ToTable($"_{nameof(ActionEntity)}-{nameof(ModifierEntity)}"));
            builder.HasDiscriminator(x => x.EntityType)
            .HasValue<ComboPointActionEntity>(ActionEntityType.ComboPoint)
            .HasValue<DamageActionEntity>(ActionEntityType.Damage)
            .HasValue<EffectActionEntity>(ActionEntityType.Effect)
            .HasValue<HealActionEntity>(ActionEntityType.Heal);
        }

        new public class DTO : SingleEntity.DTO
        {
            public ActionEntityType EntityType { get; set; }
            public bool Periodic { get; set; }
            public IEnumerable<ModifierEntity.DTO> ModifierList { get; set; }

            public DTO(ActionEntity entity) : base(entity)
            {
                EntityType = entity.EntityType;
                Periodic = entity.Periodic;
                ModifierList = entity.ModifierList.Select(x => x.ToDTO());
            }
        }
    }

    public enum ActionEntityType
    {
        ComboPoint = 0,
        Damage = 1,
        Effect = 2,
        Heal = 3,
    }
}
