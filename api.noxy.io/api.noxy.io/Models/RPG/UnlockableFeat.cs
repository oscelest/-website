using Database.Models.RPG.Abstract;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Models.RPG
{
    [Table("UnlockableFeat")]
    public class UnlockableFeat : Unlockable
    {
        [Required]
        public required TemplateFeat TemplateFeat { get; set; }
    }
}
