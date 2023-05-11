using Database.Enums;
using Database.Models.RPG.Abstract;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Models.RPG
{
    [Table("ModifierFeat")]
    public class ModifierFeat : Modifier
    {
        [Required]
        public required TemplateFeat TemplateFeat {get;set;}

        [Required]
        [Column(nameof(ModifierTagType), TypeName = "varchar(32)")]
        public required ModifierTagType ModifierTag { get; set; }
    }
}
