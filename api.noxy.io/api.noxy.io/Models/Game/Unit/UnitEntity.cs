using api.noxy.io.Models.Game.Effect;
using api.noxy.io.Models.Game.Skill;
using api.noxy.io.Models.Game.Trigger;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace api.noxy.io.Models.Game.Unit
{
    public class UnitEntity : SimpleEntity
    {
        public string Name { get; set; } = string.Empty;
        public int Experience { get; set; } = 0;
        public GuildEntity Guild { get; set; } = new();
        public UnitClassEntity Class { get; set; } = new();
        public List<SkillEntity> SkillList { get; set; } = new();

        new public DTO ToDTO() => new(this);

        public static void AddTableToBuilder(EntityTypeBuilder<UnitEntity> builder)
        {
            builder.ToTable(nameof(UnitEntity));
            builder.Property(x => x.Name).IsRequired().HasMaxLength(64);
            builder.Property(x => x.Experience).IsRequired().HasDefaultValue(0);
            builder.HasIndex(x => x.Name).IsUnique();
            builder.HasOne(x => x.Class);
            builder.HasOne(x => x.Guild).WithMany();
            builder.HasMany(x => x.SkillList).WithMany().UsingEntity(x => x.ToTable($"_{nameof(UnitEntity)}-{nameof(SkillEntity)}"));
        }

        new public class DTO : SimpleEntity.DTO
        {
            public string Name { get; set; }
            public int Experience { get; set; }
            public UnitClassEntity.DTO Class { get; set; }
            public IEnumerable<SkillEntity.DTO> SkillList { get; set; }
            public GuildEntity.DTO Guild { get; set; }

            public DTO(UnitEntity entity) : base(entity)
            {
                Name = entity.Name;
                Experience = entity.Experience;
                Guild = entity.Guild.ToDTO();
                Class = entity.Class.ToDTO();
                SkillList = entity.SkillList.Select(x => x.ToDTO());
            }
        }
    }
}
