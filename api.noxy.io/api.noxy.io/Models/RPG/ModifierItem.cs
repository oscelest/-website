using Database.Models.RPG.Abstract;
using Database.Utilities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Models.RPG
{
    [Table("ModifierItem")]
    public class ModifierItem : Modifier
    {
        [Required]
        [Column(nameof(AttributeTag), TypeName = "varchar(32)")]
        public required ItemAttributeTagType AttributeTag { get; set; }
    }
}
