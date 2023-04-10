using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.noxy.io.Models.RPG.Junction
{
    [Table("$UnitWithRole")]
    [PrimaryKey(nameof(Unit), nameof(Role))]
    public class UnitWithRole
    {
        [Required]
        public required Unit Unit { get; set; }

        [Required]
        public required Role Role { get; set; }

        [Required]
        public required int Experience { get; set; }

        [Required]
        public DateTime TimeAcquired { get; set; }
    }
}
