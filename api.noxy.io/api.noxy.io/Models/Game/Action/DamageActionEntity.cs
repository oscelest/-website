using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace api.noxy.io.Models.Game.Action
{
    public class DamageActionEntity : ActionEntity
    {
        public bool Direct { get; set; } = true;
        public DamageSourceType DamageSource { get; set; } = DamageSourceType.Attack;
        public DamageElementType DamageElement { get; set; } = DamageElementType.Physical;

        public DamageActionEntity() : base(ActionEntityType.Damage) { }

        new public DTO ToDTO() => new(this);

        public static void AddTableToBuilder(EntityTypeBuilder<DamageActionEntity> builder)
        {
            builder.Property(x => x.Direct).IsRequired().HasDefaultValue(true).HasColumnName("Direct");
            builder.Property(x => x.DamageSource).IsRequired().HasConversion<string>();
            builder.Property(x => x.DamageElement).IsRequired().HasConversion<string>();
        }

        new public class DTO : ActionEntity.DTO
        {
            public bool Direct { get; set; }
            public string DamageSource { get; set; } = DamageSourceType.Attack.ToString();
            public string DamageElement { get; set; } = DamageElementType.Physical.ToString();

            public DTO(DamageActionEntity entity) : base(entity)
            {
                Direct = entity.Direct;
                DamageSource = entity.DamageSource.ToString();
                DamageElement = entity.DamageElement.ToString();
            }
        }
    }

    public enum DamageSourceType
    {
        Attack,
        Spell
    }

    public enum DamageElementType
    {
        Physical,
        Fire,
        Cold,
        Lightning,
    }
}
