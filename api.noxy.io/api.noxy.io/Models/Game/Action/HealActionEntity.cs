using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace api.noxy.io.Models.Game.Action
{
    public class HealActionEntity : ActionEntity
    {
        public bool Direct { get; set; } = true;
        public bool Reviving { get; set; } = false;

        public HealActionEntity() : base(ActionEntityType.Heal) { }

        new public DTO ToDTO() => new(this);

        public static void AddTableToBuilder(EntityTypeBuilder<HealActionEntity> builder)
        {
            builder.Property(x => x.Direct).IsRequired().HasDefaultValue(true).HasColumnName("Direct");
            builder.Property(x => x.Reviving).IsRequired().HasDefaultValue(false);
        }

        new public class DTO : ActionEntity.DTO
        {
            public bool Direct { get; set; }
            public bool Reviving { get; set; }

            public DTO(HealActionEntity entity) : base(entity)
            {
                Direct = entity.Direct;
                Reviving = entity.Reviving;
            }
        }
    }
}
