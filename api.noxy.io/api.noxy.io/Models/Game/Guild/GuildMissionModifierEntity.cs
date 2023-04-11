using api.noxy.io.Utilities;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.noxy.io.Models.Game.Guild
{
    [Table("GuildMissionModifier")]
    public class GuildMissionModifierEntity : GuildModifierEntity
    {
        [Required]
        [Column(nameof(Tag), TypeName = "varchar(32)")]
        public ModifierGuildMissionTagType Tag { get; set; }

        #region -- DTO --

        new public DTO ToDTO() => new(this);

        new public class DTO : GuildModifierEntity.DTO
        {
            public ModifierGuildMissionTagType Tag { get; set; }

            public DTO(GuildMissionModifierEntity entity) : base(entity)
            {
                Tag = entity.Tag;
            }
        }

        #endregion -- DTO --

    }
}
