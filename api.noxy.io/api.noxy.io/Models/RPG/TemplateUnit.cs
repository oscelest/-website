using System.ComponentModel.DataAnnotations.Schema;

namespace api.noxy.io.Models.RPG
{
    [Table("TemplateUnit")]
    public class TemplateUnit : Template
    {
        #region -- Mappings --

        public List<Unit> UnitList { get; set; } = new();

        #endregion -- Mappings --
    }
}
