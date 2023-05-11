using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Models.RPG.Abstract
{
    [Table("ActionComboPoint")]
    public class ActionComboPoint : Action
    {
        [Required]
        public required int BaseValue { get; set; }

        [Required] 
        public required bool Retained { get; set; }

        [Required]
        public required bool Periodic { get; set; }
    }
}
