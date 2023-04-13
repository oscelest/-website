using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.noxy.io.Models.RPG
{
    [Table("UnlockableFeat")]
    public class UnlockableFeat : Unlockable
    {
        [Required]
        public required TemplateFeat TemplateFeat { get; set; }
    }
}
