using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace api.noxy.io.Models.Game.Modifier
{
    public class AttributeModifierEntity : ModifierEntity
    {
        public AttributeType Type { get; set; }

        public AttributeModifierEntity() : this(ModifierCategoryType.Damage) { }
        public AttributeModifierEntity(ModifierCategoryType category) : this(category, AttributeType.Health) { }
        public AttributeModifierEntity(ModifierCategoryType category, AttributeType type) : base(ModifierEntityType.Attribute, category)
        {
            Type = type;
        }

        new public DTO ToDTO() => new(this);

        public static void AddTableToBuilder(EntityTypeBuilder<AttributeModifierEntity> builder)
        {
            builder.Property(e => e.Type).HasConversion<string>().HasColumnName("Type");
        }

        new public class DTO : ModifierEntity.DTO
        {
            public AttributeType Type { get; set; }

            public DTO(AttributeModifierEntity entity) : base(entity)
            {
                Type = entity.Type;
            }
        }
    }

    public enum AttributeType
    {
        Health,
        AttackPower,
        SpellPower,
        Armor,
        Ward,
    }
}
