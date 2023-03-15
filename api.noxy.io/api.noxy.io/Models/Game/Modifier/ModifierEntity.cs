using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace api.noxy.io.Models.Game.Modifier
{
    public abstract class ModifierEntity : SimpleEntity
    {
        public ModifierEntityType EntityType { get; set; }
        public ModifierCategoryType Category { get; set; }
        public int Value { get; set; } = 0;
        public int ValuePerLevel { get; set; } = 0;

        new public DTO ToDTO() => new(this);

        protected ModifierEntity(ModifierEntityType type, ModifierCategoryType category)
        {
            EntityType = type;
            Category = category;
        }

        public static void AddTableToBuilder(EntityTypeBuilder<ModifierEntity> builder)
        {
            builder.ToTable(nameof(ModifierEntity));
            builder.Property(e => e.Category).HasConversion<string>();
            builder.Property(e => e.Value).HasDefaultValue(0).IsRequired();
            builder.Property(e => e.ValuePerLevel).HasDefaultValue(0).IsRequired();
            builder.HasDiscriminator(e => e.EntityType)
            .HasValue<ArithmeticalModifierEntity>(ModifierEntityType.Arithmetical)
            .HasValue<AttributeModifierEntity>(ModifierEntityType.Attribute);
        }

        new public class DTO : SimpleEntity.DTO
        {
            public ModifierEntityType EntityType { get; set; }
            public ModifierCategoryType Category { get; set; }
            public int Value { get; set; } = 0;
            public int ValuePerLevel { get; set; } = 0;

            public DTO(ModifierEntity entity) : base(entity)
            {
                EntityType = entity.EntityType;
                Category = entity.Category;
                Value = entity.Value;
                ValuePerLevel = entity.ValuePerLevel;
            }
        }
    }

    public enum ModifierEntityType
    {
        Arithmetical = 0,
        Attribute = 1,
    }

    public enum ModifierCategoryType
    {
        Damage,
        Heal,

        ChargeSkillMax,

        ComboPointChange,
        ComboPointRetain,
        ComboPointMax,

        EffectDuration,
        HitCount,

        UnitAttributeHealth,
        UnitAttributeAttackPower,
        UnitAttributeSpellPower,
        UnitAttributeArmor,
        UnitAttributeWard
    }
}
