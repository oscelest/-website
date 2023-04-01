using api.noxy.io.Models.Game.Modifier;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations;

namespace api.noxy.io.Models.Game.Trigger
{
    public class HealingReceivedTriggerEntity : TriggerEntity
    {
        [Required]
        public List<ModifierEntity> ModifierList { get; set; } = new();

        public HealingReceivedTriggerEntity() : base(TriggerEntityType.HealingReceived) { }

        new public DTO ToDTO() => new(this);

        public static void AddTableToBuilder(EntityTypeBuilder<HealingReceivedTriggerEntity> builder)
        {
            builder.HasMany(x => x.ModifierList).WithMany().UsingEntity(x => x.ToTable($"_{nameof(HealingReceivedTriggerEntity)}-{nameof(ModifierEntity)}"));
        }

        new public class DTO : SingleEntity.DTO
        {
            public IEnumerable<ModifierEntity.DTO> ModifierList { get; set; }

            public DTO(HealingReceivedTriggerEntity entity) : base(entity)
            {
                ModifierList = entity.ModifierList.Select(x => x.ToDTO());
            }
        }
    }
}
