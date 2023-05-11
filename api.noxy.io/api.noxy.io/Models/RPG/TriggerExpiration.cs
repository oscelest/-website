using Database.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Models.RPG.Abstract
{
    [Table("TriggerExpiration")]
    public class TriggerExpiration : Trigger
    {
        [Required]
        public required ExpirationTagType ExpirationTag { get; set; }
    }
}
