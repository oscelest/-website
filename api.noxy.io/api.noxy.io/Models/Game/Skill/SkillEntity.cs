using api.noxy.io.Models.Game.Modifier;
using api.noxy.io.Models.Game.Operation;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace api.noxy.io.Models.Game.Skill
{
    public class SkillEntity : SimpleEntity
    {
        public SkillEntityType EntityType { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public List<OperationEntity> OperationList { get; set; } = new();
        public List<ModifierEntity> ModifierList { get; set; } = new();

        public SkillEntity() : this(SkillEntityType.Charge) { }
        public SkillEntity(SkillEntityType type)
        {
            EntityType = type;
        }

        new public DTO ToDTO() => new(this);

        public static void AddTableToBuilder(EntityTypeBuilder<SkillEntity> builder)
        {
            builder.ToTable(nameof(SkillEntity));
            builder.Property(x => x.Name).IsRequired().HasMaxLength(64);
            builder.Property(x => x.Description).IsRequired();
            builder.Property(x => x.EntityType).IsRequired();
            builder.HasMany(x => x.OperationList).WithMany().UsingEntity(x => x.ToTable($"_{nameof(SkillEntity)}-{nameof(OperationEntity)}"));
            builder.HasMany(x => x.ModifierList).WithMany().UsingEntity(x => x.ToTable($"_{nameof(SkillEntity)}-{nameof(ModifierEntity)}"));
        }

        new public class DTO : SimpleEntity.DTO
        {
            public string EntityType { get; set; }
            public string Name { get; set; } = string.Empty;
            public string? Description { get; set; }
            public IEnumerable<OperationEntity.DTO> OperationList { get; set; }
            public IEnumerable<ModifierEntity.DTO> ModifierList { get; set; }

            public DTO(SkillEntity entity) : base(entity)
            {
                EntityType = entity.EntityType.ToString();
                Name = entity.Name;
                Description = entity.Description;
                OperationList = entity.OperationList.Select(x => x.ToDTO());
                ModifierList = entity.ModifierList.Select(x => x.ToDTO());
            }
        }
    }

    public enum SkillEntityType
    {
        Charge = 0,
        Combo = 1,
    }
}
