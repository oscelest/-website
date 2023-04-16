using System.ComponentModel.DataAnnotations.Schema;

namespace api.noxy.io.Models.RPG
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
