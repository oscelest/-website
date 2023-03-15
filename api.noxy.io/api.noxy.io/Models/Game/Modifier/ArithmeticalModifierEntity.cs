using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace api.noxy.io.Models.Game.Modifier
{
    public class ArithmeticalModifierEntity : ModifierEntity
    {
        public ArithmeticalType Type { get; set; }

        public ArithmeticalModifierEntity() : this(ModifierCategoryType.Damage) { }
        public ArithmeticalModifierEntity(ModifierCategoryType category) : this(category, ArithmeticalType.Additive) { }
        public ArithmeticalModifierEntity(ModifierCategoryType category, ArithmeticalType type) : base(ModifierEntityType.Arithmetical, category)
        {
            Type = type;
        }

        new public DTO ToDTO() => new(this);

        public static void AddTableToBuilder(EntityTypeBuilder<ArithmeticalModifierEntity> builder)
        {
            builder.Property(e => e.Type).HasConversion<string>().HasColumnName("Type");
        }

        new public class DTO : ModifierEntity.DTO
        {
            public ArithmeticalType Type { get; set; }
       
            public DTO(ArithmeticalModifierEntity entity) : base(entity)
            {
                Type = entity.Type;
            }
        }
    }

    public enum ArithmeticalType
    {
        Additive,
        Multiplicative,
        Exponential,
    }
}
