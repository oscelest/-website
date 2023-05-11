using Database.Models.RPG.Abstract;
using Database.Utilities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Models.RPG
{
    [Table("ModifierRole")]
    public class ModifierRole : Modifier
    {
        [Required]
        [Column(nameof(RoleTag), TypeName = "varchar(32)")]
        public required ModifierGuildRoleTagType RoleTag { get; set; }

        public TemplateRole? TemplateRole { get; set; }
    }
}
