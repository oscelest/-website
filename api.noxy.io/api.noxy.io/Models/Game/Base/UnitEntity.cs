using api.noxy.io.Models.Game.Unit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace api.noxy.io.Models.Game.Base
{
    public class UnitEntity : SimpleEntity
    {
        public string Name { get; set; } = string.Empty;
        public int Experience { get; set; } = 0;
        public bool Recruited { get; set; } = false;
        public List<AffinityEntity> AffinityList { get; set; } = new();
        public List<ProfessionEntity> ProfessionList { get; set; } = new();

        new public DTO ToDTO() => new(this);

        public static void AddTableToBuilder(EntityTypeBuilder<UnitEntity> builder)
        {
            builder.ToTable(nameof(UnitEntity));
            builder.Property(x => x.Name).IsRequired().HasMaxLength(64);
            builder.Property(x => x.Experience).IsRequired().HasDefaultValue(0);
            builder.Property(x => x.Recruited).IsRequired().HasDefaultValue(false);
            builder.HasIndex(x => x.Name).IsUnique();
            builder.HasMany(x => x.AffinityList).WithMany().UsingEntity(x => x.ToTable($"_{nameof(UnitEntity)}-{nameof(AffinityEntity)}"));
            builder.HasMany(x => x.ProfessionList).WithMany().UsingEntity(x => x.ToTable($"_{nameof(UnitEntity)}-{nameof(ProfessionEntity)}"));
        }

        new public class DTO : SimpleEntity.DTO
        {
            public string Name { get; set; }
            public int Experience { get; set; }
            public IEnumerable<AffinityEntity.DTO> AffinityList { get; set; }
            public IEnumerable<ProfessionEntity.DTO> ProfessionList { get; set; }

            public DTO(UnitEntity entity) : base(entity)
            {
                Name = entity.Name;
                Experience = entity.Experience;
                AffinityList = entity.AffinityList.Select(x => x.ToDTO());
                ProfessionList = entity.ProfessionList.Select(x => x.ToDTO());
            }
        }
    }
}
