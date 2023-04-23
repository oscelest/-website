using System.ComponentModel.DataAnnotations.Schema;

namespace api.noxy.io.Models.RPG
{
    [Table("TemplateUnit")]
    public class TemplateUnit : Template
    {
        public List<SlotGear> SlotGearList { get; set; } = new(); 

        #region -- Mappings --

        public List<Unit> UnitList { get; set; } = new();

        #endregion -- Mappings --
    }
}
