using api.noxy.io.Models.Game.Effect;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace api.noxy.io.Models.Game.Action
{
    public class EffectActionEntity : ActionEntity
    {
        public EffectEntity Effect { get; set; } = new();

        public EffectActionEntity() : base(ActionEntityType.Effect) { }

        new public DTO ToDTO() => new(this);

        public static void AddTableToBuilder(EntityTypeBuilder<EffectActionEntity> builder)
        {
            builder.HasOne(x => x.Effect).WithMany();
        }

        new public class DTO : ActionEntity.DTO
        {
            public EffectEntity.DTO Effect { get; set; }

            public DTO(EffectActionEntity entity) : base(entity)
            {
                Effect = entity.Effect.ToDTO();
            }
        }
    }
}
