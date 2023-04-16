﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.noxy.io.Models.RPG
{
    [Table("TemplateItemAugmentation")]
    public class TemplateItemAugmentation : TemplateItem
    {
        [Required]
        public required TemplateSlot TemplateSlot { get; set; }

        public required List<ModifierItem> ModifierItemList { get; set; }

        #region -- Mappings --

        public List<ItemAugmentation> ItemAugmentationList { get; set; } = new();

        #endregion -- Mappings --
    }
}
