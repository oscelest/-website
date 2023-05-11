using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Models.RPG.Abstract
{
    [Table("TriggerDamaged")]
    public class TriggerDamaged : Trigger
    {
        public List<Modifier> ModifierList { get; set; } = new();
    }
}
