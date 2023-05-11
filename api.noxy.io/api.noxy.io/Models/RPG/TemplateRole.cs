using Database.Models.RPG.Abstract;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Models.RPG
{
    [Table("TemplateRole")]
    public class TemplateRole : Template
    {
        #region -- Mappings --

        public List<Role> RoleList { get; set; } = new();
        public List<UnlockableRole> UnlockableRoleList { get; set; } = new();

        #endregion -- Mappings --
    }
}
