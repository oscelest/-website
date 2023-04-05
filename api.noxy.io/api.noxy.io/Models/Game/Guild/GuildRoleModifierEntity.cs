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

        public RoleTypeEntity? RoleType { get; set; }

        public class Set
        {
            public int Count;
            public int Experience;

            public Set(int count, int experience)
            {
                Count = count;
                Experience = experience;
            }
        }

        #region -- DTO --

        new public DTO ToDTO() => new(this);

        new public class DTO : GuildModifierEntity.DTO
        {
            public GuildRoleModifierTagType Tag { get; set; }
            public RoleTypeEntity.DTO? RoleType { get; set; }

            public DTO(GuildRoleModifierEntity entity) : base(entity)
            {
                Tag = entity.Tag;
                RoleType = entity.RoleType?.ToDTO();
            }
        }

        #endregion -- DTO --

    }
}
