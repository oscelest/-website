using api.noxy.io.Models.Auth;
using api.noxy.io.Utilities;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.noxy.io.Models.Game.Guild
{
    [Table("Guild")]
    [Index(nameof(Name), IsUnique = true)]
    public class GuildEntity : SingleEntity
    {
        [Required]
        [MinLength(3), MaxLength(64)]
        public required string Name { get; set; }

        [Required]
        [DefaultValue(0)]
        public required int Currency { get; set; }

        [Required]
        public required UserEntity User { get; set; }

        public DateTime? TimeUnitRefresh { get; set; }

        public DateTime? TimeMissionRefresh { get; set; }


        #region -- Mapping --

        public List<UnitEntity> UnitList { get; set; } = new();
        public List<MissionEntity> MissionList { get; set; } = new();
        public List<GuildFeatEntity> GuildFeatList { get; set; } = new();

        #endregion -- Mapping --

        public float GetModifierValue<T>(Func<T, bool> fn) where T : GuildModifierEntity => GetModifierValue(0, fn);
        public float GetModifierValue<T>(float flat, Func<T, bool> fn) where T : GuildModifierEntity
        {
            float additive = 0f;
            float multiplicative = 1f;
            float exponential = 1f;

            foreach (GuildFeatEntity junction in GuildFeatList)
            {
                foreach (GuildModifierEntity gMod in junction.Feat.GuildModifierList)
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

        new public class DTO : SingleEntity.DTO
        {
            public string Name { get; set; }
            public int Currency { get; set; }
            public DateTime? TimeUnitRefresh { get; set; }
            public DateTime? TimeMissionRefresh { get; set; }

            public DTO(GuildEntity entity) : base(entity)
            {
                Name = entity.Name;
                Currency = entity.Currency;
                TimeUnitRefresh = entity.TimeUnitRefresh;
                TimeMissionRefresh = entity.TimeMissionRefresh;
            }
        }

        #endregion -- DTO --

    }
}
