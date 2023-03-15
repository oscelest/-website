using api.noxy.io.Models.Game.Modifier;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace api.noxy.io.Models.Game.Unit
{
    public class UnitClassEntity : SimpleEntity
    {
        public string Name { get; set; } = string.Empty;
        public List<ModifierEntity> ModifierList { get; set; } = new();

        new public DTO ToDTO() => new(this);

        public static void AddTableToBuilder(EntityTypeBuilder<UnitClassEntity> builder)
        {
            builder.ToTable(nameof(UnitClassEntity));
            builder.Property(x => x.Name).IsRequired().HasMaxLength(64);
            builder.HasIndex(x => x.Name).IsUnique();
            builder.HasMany(x => x.ModifierList).WithOne();
        }

        new public class DTO : SimpleEntity.DTO
        {
            public string Name { get; set; }
            public IEnumerable<ModifierEntity.DTO> ModifierList { get; set; }

            public DTO(UnitClassEntity entity) : base(entity)
            {
                Name = entity.Name;
                ModifierList = entity.ModifierList.Select(x => x.ToDTO());
            }
        }
    }
}
