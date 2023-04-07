using api.noxy.io.Models.Auth;
using api.noxy.io.Utilities;
using Microsoft.EntityFrameworkCore;
using System;
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
        [Column(nameof(State), TypeName = "varchar(32)")]
        public GuildStateType State { get; set; }

        [Required]
        public required UserEntity User { get; set; }

        public DateTime? TimeUnitRefresh { get; set; }

        public DateTime? TimeMissionRefresh { get; set; }

        public List<UnitEntity> UnitList { get; set; } = new();
        
        public List<MissionEntity> MissionList { get; set; } = new();
        
        public List<GuildFeatEntity> GuildFeatList { get; set; } = new();
        
        public List<GuildRoleEntity> GuildRoleList { get; set; } = new();

        public GuildRoleModifierEntity.Set GetRoleModifierSet(Guid? type = null)
        {
            int count = (int)GetRoleModifierValue(GuildRoleModifierTagType.Count, type);
            int experience = (int)GetRoleModifierValue(GuildRoleModifierTagType.Experience, type);
            return new GuildRoleModifierEntity.Set(count, experience);
        }

        public float GetRoleModifierValue(GuildRoleModifierTagType tag, Guid? type = null, float flat = 0f) => GetModifierValue<GuildRoleModifierEntity>(flat, x => x.RoleType?.ID == type && x.Tag == tag);

        public float GetUnitModifierValue(GuildUnitModifierTagType tag, float flat = 0f) => GetModifierValue<GuildUnitModifierEntity>(flat, x => x.Tag == tag);

        public float GetUnitModifierValue(GuildMissionModifierTagType tag, float flat = 0f) => GetModifierValue<GuildMissionModifierEntity>(flat, x => x.Tag == tag);

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
            public GuildStateType State { get; set; }
            public DateTime? TimeUnitRefresh { get; set; }
            public DateTime? TimeMissionRefresh { get; set; }

            public DTO(GuildEntity entity) : base(entity)
            {
                Name = entity.Name;
                Currency = entity.Currency;
                State = entity.State;
                TimeUnitRefresh = entity.TimeUnitRefresh;
                TimeMissionRefresh = entity.TimeMissionRefresh;
            }
        }

        #endregion -- DTO --

    }
}
