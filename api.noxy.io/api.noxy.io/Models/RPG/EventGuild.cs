using api.noxy.io.Utilities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.noxy.io.Models.RPG
{
    [Table("EventGuild")]
    public class EventGuild : Event
    {
        [Required]
        [Column(nameof(Tag), TypeName = "varchar(32)")]
        public required EventTagType Tag { get; set; }

        public required Guild Guild { get; set; }
    }

}
