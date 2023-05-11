using System.ComponentModel.DataAnnotations;

namespace Database.Models.RPG.Abstract
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
    }
}
