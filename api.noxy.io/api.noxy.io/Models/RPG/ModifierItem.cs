using api.noxy.io.Utilities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.noxy.io.Models.RPG
{
    [Table("ModifierItem")]
    public class ModifierItem : Modifier
    {
        [Required]
        [Column(nameof(AttributeTag), TypeName = "varchar(32)")]
        public required ItemAttributeTagType AttributeTag { get; set; }
    }
}
