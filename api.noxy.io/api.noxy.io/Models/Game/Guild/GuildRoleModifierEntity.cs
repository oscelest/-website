using api.noxy.io.Utilities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.noxy.io.Models.Game.Guild
{
    [Table("GuildRoleModifier")]
    public class GuildRoleModifierEntity : GuildModifierEntity
    {
        [Required]
        [Column(nameof(Tag), TypeName = "varchar(32)")]
        public required GuildRoleModifierTagType Tag { get; set; }

        // Mappings
        public RoleTypeEntity? RoleType { get; set; } = new();

        #region -- DTO --

        new public DTO ToDTO() => new(this);

        new public class DTO : GuildModifierEntity.DTO
        {
            public GuildRoleModifierTagType Tag { get; set; }
            public RoleTypeEntity? RoleType { get; set; }

            public DTO(GuildRoleModifierEntity entity) : base(entity)
            {
                Tag = entity.Tag;
                RoleType = entity.RoleType;
            }
        }

        #endregion -- DTO --

    }
}
