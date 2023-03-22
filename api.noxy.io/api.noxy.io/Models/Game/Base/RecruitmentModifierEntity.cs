using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace api.noxy.io.Models.Game.Modifier
{
    public class RecruitmentModifierEntity : SimpleEntity
    {
        public int Value { get; set; } = 0;
        public TagType Tag { get; set; }
        public ArithmaticalType Arithmatical { get; set; }

        public RecruitmentModifierEntity() : this(TagType.ExperienceLevel) { }
        public RecruitmentModifierEntity(TagType tag) : this(tag, ArithmaticalType.Additive) { }
        public RecruitmentModifierEntity(TagType tag, ArithmaticalType arithmatical)
        {
            Tag = tag;
            Arithmatical = arithmatical;
        }

        new public DTO ToDTO() => new(this);

        public static void AddTableToBuilder(EntityTypeBuilder<RecruitmentModifierEntity> builder)
        {
            builder.ToTable(nameof(RecruitmentModifierEntity));
            builder.Property(e => e.Value).HasDefaultValue(0).IsRequired();
            builder.Property(e => e.Tag).HasConversion<string>().IsRequired();
            builder.Property(e => e.Arithmatical).HasConversion<string>().IsRequired();
        }

        new public class DTO : SimpleEntity.DTO
        {
            public int Value { get; set; }
            public TagType Tag { get; set; }
            public ArithmaticalType Arithmatical { get; set; }

            public DTO(RecruitmentModifierEntity entity) : base(entity)
            {
                Value = entity.Value;
                Tag = entity.Tag;
                Arithmatical = entity.Arithmatical;
            }
        }

        public enum TagType
        {
            RefreshRate,
            UnitCount,
            ExperienceLevel,
            ProfessionLevel,
            AffinityLevel,
        }

        public enum ArithmaticalType
        {
            Additive,
            Multiplicative,
            Exponential,
        }
    }
}
