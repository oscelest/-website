using api.noxy.io.Models.Game.Modifier;
using api.noxy.io.Models.Game.Trigger;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace api.noxy.io.Models.Game.Effect
{
    public class EffectEntity : SimpleEntity
    {
        public string Name { get; set; } = string.Empty;
        public bool Expires { get; set; } = true;
        public bool Removable { get; set; } = true;
        public EffectAlignmentType Alignment { get; set; } = EffectAlignmentType.Postive;
        public List<TriggerEntity> TriggerList { get; set; } = new();
        public List<ModifierEntity> ModifierList { get; set; } = new();

        new public DTO ToDTO() => new(this);

        public static void AddTableToBuilder(EntityTypeBuilder<EffectEntity> builder)
        {
            builder.ToTable(nameof(EffectEntity));
            builder.Property(x => x.Name).IsRequired().HasMaxLength(64);
            builder.Property(x => x.Expires).IsRequired().HasDefaultValue(true);
            builder.Property(x => x.Removable).IsRequired().HasDefaultValue(true);
            builder.Property(x => x.Alignment).IsRequired().HasConversion<string>();
            builder.HasMany(x => x.TriggerList).WithMany().UsingEntity(x => x.ToTable($"_{nameof(EffectEntity)}-{nameof(TriggerEntity)}"));
            builder.HasMany(x => x.ModifierList).WithMany().UsingEntity(x => x.ToTable($"_{nameof(EffectEntity)}-{nameof(ModifierEntity)}"));
        }

        new public class DTO : SimpleEntity.DTO
        {
            public string Name { get; set; }
            public bool Expires { get; set; }
            public bool Removable { get; set; }
            public string Alignment { get; set; }
            public IEnumerable<TriggerEntity.DTO> TriggerList { get; set; }
            public IEnumerable<ModifierEntity.DTO> ModifierList { get; set; }

            public DTO(EffectEntity entity) : base(entity)
            {
                Name = entity.Name;
                Expires = entity.Expires;
                Removable = entity.Removable;
                Alignment = entity.Alignment.ToString();
                TriggerList = entity.TriggerList.Select(x => x.ToDTO());
                ModifierList = entity.ModifierList.Select(x => x.ToDTO());
            }
        }
    }

    public enum EffectAlignmentType
    {
        Postive = 0,
        Neutral = 1,
        Negative = 2
    }
}
