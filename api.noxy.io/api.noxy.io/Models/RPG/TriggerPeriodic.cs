using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Models.RPG.Abstract
{
    [Table("TriggerPeriodic")]
    public class TriggerPeriodic : Trigger
    {
        [Required]
        public required int Interval { get; set; }
    }
}
