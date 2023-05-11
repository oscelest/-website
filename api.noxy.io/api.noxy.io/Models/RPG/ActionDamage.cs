using Database.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Models.RPG.Abstract
{
    [Table("ActionDamage")]
    public class ActionDamage : Action
    {
        [Required]
        public required DamageSourceTagType DamageSourceTag { get; set; }

        [Required]
        public required DamageElementTagType DamageElementTag { get; set; }

        [Required]
        public required bool Direct { get; set; }

        [Required]
        public required bool Periodic { get; set; }
    }
}
