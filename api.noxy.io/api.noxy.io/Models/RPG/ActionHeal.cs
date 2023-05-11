using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Models.RPG.Abstract
{
    [Table("ActionHeal")]
    public class ActionHeal : Action
    {
        [Required]
        public required bool Direct { get; set; }

        [Required]
        public required bool Periodic { get; set; }

        [Required]
        public required bool Reviving { get; set; }
    }
}
