using api.noxy.io.Utilities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.noxy.io.Models.RPG
{
    [Table("ModifierGuildUnit")]
    public class ModifierGuildUnit : ModifierGuild
    {
        [Required]
        [Column(nameof(UnitTag), TypeName = "varchar(32)")]
        public required ModifierGuildUnitTagType UnitTag { get; set; }
    }
}
