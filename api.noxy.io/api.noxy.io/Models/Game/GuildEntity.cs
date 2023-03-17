using api.noxy.io.Models.Game.Unit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace api.noxy.io.Models.Game
{
    public class GuildEntity : SimpleEntity
    {
        public string Name { get; set; } = string.Empty;
        public UserEntity User { get; set; } = new();
        public int Currency { get; set; } = 100;
        public List<UnitEntity> UnitList { get; set; } = new();

        new public DTO ToDTO() => new(this);

        public static void AddTableToBuilder(EntityTypeBuilder<GuildEntity> builder)
        {
            builder.ToTable(nameof(GuildEntity));
            builder.Property(x => x.Name).IsRequired().HasMaxLength(64);
            builder.HasIndex(x => x.Name).IsUnique();
            builder.HasOne(x => x.User);
            builder.HasMany(x => x.UnitList).WithOne();
        }

        new public class DTO : SimpleEntity.DTO
        {
            public string Name { get; set; }
            public UserEntity.DTO User { get; set; }
            public IEnumerable<UnitEntity.DTO> UnitList { get; set; }

            public DTO(GuildEntity entity) : base(entity)
            {
                Name = entity.Name;
                User = entity.User.ToDTO();
                UnitList = entity.UnitList.Select(x => x.ToDTO());
            }
        }
    }
}
