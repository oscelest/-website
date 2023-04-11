using api.noxy.io.Models.Game.Feat;
using api.noxy.io.Utilities;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.noxy.io.Models.RPG
{
    public abstract class Event
    {
        [Key()]
        public Guid ID { get; set; } = Guid.NewGuid();

        [Required]
        public required int Count { get; set; }

        [Required]
        public required DateTime TimeFirstOccurrence { get; set; }

        [Required]
        public required DateTime TimeLastOccurrence { get; set; }

        public class Guild : Event
        {
            [Required]
            [Column(nameof(Tag), TypeName = "varchar(32)")]
            public required EventTagType Tag { get; set; }

            public required Guild GuildRef { get; set; }
        }

    }
}
