using Database.Models.RPG.Abstract;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Models.RPG
{
    [Table("SlotGear")]
    public class SlotGear : Slot
    {
        #region -- Mappings --

        public List<TemplateUnit> TemplateUnitList { get; set; } = new();

        #endregion -- Mappings --
    }
}
