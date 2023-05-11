using Database.Models.RPG.Abstract;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Models.RPG
{
    [Table("TemplateUnit")]
    public class TemplateUnit : Template
    {
        public List<SlotGear> SlotGearList { get; set; } = new();

        public List<Modifier> ModifierList { get; set; } = new();

        #region -- Mappings --

        public List<Unit> UnitList { get; set; } = new();

        #endregion -- Mappings --
    }
}
