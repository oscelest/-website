using api.noxy.io.Utilities;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.noxy.io.Models.Game.Guild
{
    [Table("Guild")]
    [Index(nameof(Name), IsUnique = true)]
    public class GuildEntity : SimpleEntity
    {
        [Required]
        [MinLength(3), MaxLength(64)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [DefaultValue(0)]
        public int Currency { get; set; } = 0;

        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime TimeUnitRefresh { get; set; } = DateTime.MinValue;

        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime TimeMissionRefresh { get; set; } = DateTime.MinValue;

        // Mappings
        public UserEntity User { get; set; } = new();
        public List<UnitEntity> UnitList { get; set; } = new();
        public List<MissionEntity> MissionList { get; set; } = new();
        public List<FeatEntity> FeatList { get; set; } = new();

        public float GetModifierValue<T>(Func<T, bool> fn) where T : GuildModifierEntity => GetModifierValue(0, fn);
        public float GetModifierValue<T>(float flat, Func<T, bool> fn) where T : GuildModifierEntity
        {
            float additive = 0f;
            float multiplicative = 1f;
            float exponential = 1f;

            foreach (FeatEntity feat in FeatList)
            {
                foreach (GuildModifierEntity gMod in feat.GuildModifierList)
                {
                    if (gMod is T rMod && fn(rMod))
                    {
                        if (rMod.ArithmeticalTag == ArithmeticalTagType.Additive)
                        {
                            additive += rMod.Value;
                        }
                        else if (rMod.ArithmeticalTag == ArithmeticalTagType.Multiplicative)
                        {
                            multiplicative += rMod.Value;
                        }
                        else if (rMod.ArithmeticalTag == ArithmeticalTagType.Exponential)
                        {
                            exponential *= rMod.Value;
                        }
                    }
                }
            }

            return (flat + additive) * multiplicative * exponential;
        }

        #region -- DTO --

        new public DTO ToDTO() => new(this);

        new public class DTO : SimpleEntity.DTO
        {
            public string Name { get; set; }
            public UserEntity.DTO User { get; set; }
            public IEnumerable<UnitEntity.DTO>? UnitList { get; set; }
            public IEnumerable<MissionEntity.DTO>? MissionList { get; set; }

            public DTO(GuildEntity entity) : base(entity)
            {
                Name = entity.Name;
                User = entity.User.ToDTO();
                UnitList = entity.UnitList?.Select(x => x.ToDTO());
                MissionList = entity.MissionList?.Select(x => x.ToDTO());
            }
        }

        #endregion -- DTO --

    }
}
