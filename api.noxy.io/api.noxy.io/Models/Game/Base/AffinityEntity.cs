using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace api.noxy.io.Models.Game.Base
{
    public class AffinityEntity : SimpleEntity
    {
        public string Name { get; set; } = string.Empty;
        public List<FeatEntity> FeatList { get; set; } = new();

        new public DTO ToDTO() => new(this);

        public static void AddTableToBuilder(EntityTypeBuilder<AffinityEntity> builder)
        {
            builder.ToTable(nameof(AffinityEntity));
            builder.Property(x => x.Name).IsRequired().HasMaxLength(64);
            builder.HasIndex(x => x.Name).IsUnique();
            builder.HasMany(x => x.FeatList).WithMany().UsingEntity(x => x.ToTable($"_{nameof(AffinityEntity)}-{nameof(FeatEntity)}"));
        }

        new public class DTO : SimpleEntity.DTO
        {
            public string Name { get; set; }
            public IEnumerable<FeatEntity.DTO> FeatList { get; set; }

            public DTO(AffinityEntity entity) : base(entity)
            {
                Name = entity.Name;
                FeatList = entity.FeatList.Select(x => x.ToDTO());
            }
        }
    }
}
