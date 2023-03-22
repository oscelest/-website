using api.noxy.io.Models.Game.Modifier;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace api.noxy.io.Models.Game.Base
{
    public class FeatEntity : SimpleEntity
    {
        public string Name { get; set; } = string.Empty;
        public List<RecruitmentModifierEntity> RecruitmentModifierList { get; set; } = new();

        new public DTO ToDTO() => new(this);

        public static void AddTableToBuilder(EntityTypeBuilder<FeatEntity> builder)
        {
            builder.ToTable(nameof(FeatEntity));
            builder.Property(x => x.Name).IsRequired().HasMaxLength(64);
            builder.HasIndex(x => x.Name).IsUnique();
            builder.HasMany(x => x.RecruitmentModifierList).WithMany().UsingEntity(x => x.ToTable($"_{nameof(FeatEntity)}-{nameof(RecruitmentModifierEntity)}"));
        }

        new public class DTO : SimpleEntity.DTO
        {
            public string Name { get; set; }
            public IEnumerable<RecruitmentModifierEntity.DTO> RecruitmentModifierList { get; set; }

            public DTO(FeatEntity entity) : base(entity)
            {
                Name = entity.Name;
                RecruitmentModifierList = entity.RecruitmentModifierList.Select(x => x.ToDTO());
            }
        }
    }
}
