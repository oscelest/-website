using Database.Models.RPG.Abstract;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Models.RPG
{
    [Table("SlotSupport")]
    public class SlotSupport : Slot
    {
        #region -- Mappings --

        public List<TemplateItemGear> TemplateItemGearList { get; set; } = new();

        #endregion -- Mappings --
    }
}
