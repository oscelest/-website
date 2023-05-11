using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Models.RPG.Abstract
{
    [Table("ActionEffect")]
    public class ActionEffect : Action
    {
        [Required]
        public required Effect Effect { get; set; }
    }
}
