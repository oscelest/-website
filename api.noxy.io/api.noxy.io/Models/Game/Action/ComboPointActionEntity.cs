using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace api.noxy.io.Models.Game.Action
{
    public class ComboPointActionEntity : ActionEntity
    {
        public int BaseValue { get; set; } = 0;
        public bool Retained { get; set; } = false;

        public ComboPointActionEntity() : base(ActionEntityType.ComboPoint) { }

        new public DTO ToDTO() => new(this);

        public static void AddTableToBuilder(EntityTypeBuilder<ComboPointActionEntity> builder)
        {
            builder.Property(x => x.BaseValue).IsRequired().HasDefaultValue(0);
            builder.Property(x => x.Retained).IsRequired().HasDefaultValue(false);
        }

        new public class DTO : ActionEntity.DTO
        {
            public int BaseValue { get; set; }
            public bool Retained { get; set; }

            public DTO(ComboPointActionEntity entity) : base(entity)
            {
                BaseValue = entity.BaseValue;
                Retained = entity.Retained;
            }
        }
    }
}
