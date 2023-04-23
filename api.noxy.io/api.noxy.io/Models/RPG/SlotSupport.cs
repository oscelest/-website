using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.noxy.io.Models.RPG
{
    [Table("SlotSupport")]
    public class SlotSupport : Slot
    {
        #region -- Mappings --

        public List<TemplateItemGear> TemplateItemGearList { get; set; } = new();

        #endregion -- Mappings --
    }
}
