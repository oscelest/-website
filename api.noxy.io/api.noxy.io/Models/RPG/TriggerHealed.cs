using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Models.RPG.Abstract
{
    [Table("TriggerHealed")]
    public class TriggerHealed : Trigger
    {
        public List<Modifier> ModifierList { get; set; } = new();
    }
}
