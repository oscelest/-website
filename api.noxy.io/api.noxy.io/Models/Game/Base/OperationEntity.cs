using api.noxy.io.Models.Game.Action;
using api.noxy.io.Models.Game.Modifier;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace api.noxy.io.Models.Game.Base
{
    public class OperationEntity : SimpleEntity
    {
        public TargetType Target { get; set; } = TargetType.Self;
        public List<ActionEntity> ActionList { get; set; } = new();
        public List<ModifierEntity> ModifierList { get; set; } = new();

        new public DTO ToDTO() => new(this);

        public static void AddTableToBuilder(EntityTypeBuilder<OperationEntity> builder)
        {
            builder.ToTable(nameof(OperationEntity));
            builder.Property(x => x.Target).IsRequired().HasConversion<string>();
            builder.HasMany(x => x.ActionList).WithMany().UsingEntity(x => x.ToTable($"_{nameof(OperationEntity)}-{nameof(ActionEntity)}"));
            builder.HasMany(x => x.ModifierList).WithMany().UsingEntity(x => x.ToTable($"_{nameof(OperationEntity)}-{nameof(ModifierEntity)}"));
        }

        new public class DTO : SimpleEntity.DTO
        {
            public string Target { get; set; }
            public IEnumerable<ActionEntity.DTO> OperationList { get; set; }
            public IEnumerable<ModifierEntity.DTO> ModifierList { get; set; }

            public DTO(OperationEntity entity) : base(entity)
            {
                Target = entity.Target.ToString();
                OperationList = entity.ActionList.Select(x => x.ToDTO());
                ModifierList = entity.ModifierList.Select(x => x.ToDTO());
            }
        }
    }

    public enum TargetType
    {
        AnySingle = 0,
        AnyChain = 1,
        AnyGroup = 2,
        PlayerSingle = 3,
        PlayerChain = 4,
        PlayerGroup = 5,
        EnemySingle = 6,
        EnemyChain = 7,
        EnemyGroup = 8,
        Self = 9,
        Source = 10,
    }
}