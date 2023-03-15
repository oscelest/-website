using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace api.noxy.io.Models
{
    public abstract class SimpleEntity
    {
        public Guid ID { get; set; }

        public DateTime TimeCreated { get; set; }

        public DateTime TimeUpdated { get; set; }

        public DTO ToDTO() => new(this);

        public static void AddTableToBuilder(EntityTypeBuilder<SimpleEntity> builder)
        {
            builder.ToTable(nameof(SimpleEntity));
            builder.HasKey(x => x.ID);
            builder.Property(x => x.TimeCreated).IsRequired().ValueGeneratedOnAdd();
            builder.Property(x => x.TimeUpdated).ValueGeneratedOnUpdate();
            builder.HasIndex(x => x.TimeCreated, "IDX_TimeCreated").IsDescending();
        }

        public class DTO
        {
            public Guid ID { get; set; }
            public DateTime TimeCreated { get; set; }
            public DateTime TimeUpdated { get; set; }

            public DTO(SimpleEntity entity)
            {
                ID = entity.ID;
                TimeCreated = entity.TimeCreated;
                TimeUpdated = entity.TimeUpdated;
            }
        }
    }
}
