using Database.Models.RPG.Abstract;
using Database.Utilities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Models.RPG
{
    [Table("ModifierGuildUnit")]
    public class ModifierGuildUnit : ModifierFeat
    {
        [Required]
        [Column(nameof(UnitTag), TypeName = "varchar(32)")]
        public required ModifierGuildUnitTagType UnitTag { get; set; }
    }
}
