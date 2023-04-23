using Database.Models.RPG.Junction;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.noxy.io.Models.RPG
{
    [Table("SlotGear")]
    public class SlotGear : Slot
    {
        #region -- Mappings --

        public List<TemplateUnit> TemplateUnitList { get; set; } = new();

        #endregion -- Mappings --
    }
}
