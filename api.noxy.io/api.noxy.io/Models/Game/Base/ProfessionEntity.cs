using api.noxy.io.Models.Game.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace api.noxy.io.Models.Game.Unit
{
    public class ProfessionEntity : SimpleEntity
    {
        public string Name { get; set; } = string.Empty;
        public List<FeatEntity> FeatList { get; set; } = new();

        new public DTO ToDTO() => new(this);

        public static void AddTableToBuilder(EntityTypeBuilder<ProfessionEntity> builder)
        {
            builder.ToTable(nameof(ProfessionEntity));
            builder.Property(x => x.Name).IsRequired().HasMaxLength(64);
            builder.HasIndex(x => x.Name).IsUnique();
            builder.HasMany(x => x.FeatList).WithMany().UsingEntity(x => x.ToTable($"_{nameof(ProfessionEntity)}-{nameof(FeatEntity)}"));
        }

        new public class DTO : SimpleEntity.DTO
        {
            public string Name { get; set; }
            public IEnumerable<FeatEntity.DTO> FeatList { get; set; }

            public DTO(ProfessionEntity entity) : base(entity)
            {
                Name = entity.Name;
                FeatList = entity.FeatList.Select(x => x.ToDTO());
            }
        }
    }
}
