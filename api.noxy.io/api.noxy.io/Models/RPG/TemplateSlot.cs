﻿using Database.Models.RPG.Junction;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.noxy.io.Models.RPG
{
    [Table("TemplateSlot")]
    public class TemplateSlot : Template
    {
        #region -- Mappings --

        public List<TemplateItemGearWithTemplateSlot> TemplateItemGearWithTemplateSlotList { get; set; } = new();

        #endregion -- Mappings --
    }
}
