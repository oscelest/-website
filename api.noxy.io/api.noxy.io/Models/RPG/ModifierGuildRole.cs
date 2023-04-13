using api.noxy.io.Utilities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.noxy.io.Models.RPG
{
    [Table("ModifierGuildRole")]
    public class ModifierGuildRole : ModifierGuild
    {
        [Required]
        [Column(nameof(RoleTag), TypeName = "varchar(32)")]
        public required ModifierGuildRoleTagType RoleTag { get; set; }

        public TemplateRole? TemplateRole { get; set; }
    }
}
