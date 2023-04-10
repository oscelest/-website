using api.noxy.io.Utilities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.noxy.io.Models.Game.Guild
{
    [Table("GuildEvent")]
    public class GuildEventEntity
    {
        [Key()]
        public Guid ID { get; set; } = Guid.NewGuid();

        [Required]
        [Column(nameof(Tag), TypeName = "varchar(32)")]
        public required EventTagType Tag { get; set; }

        [Required]
        public required int Count { get; set; }

        [Required]
        public required DateTime TimeFirstOccurrence { get; set; }

        [Required]
        public required DateTime TimeLastOccurrence { get; set; }

        #region -- Mappings --

        [Required]
        public required GuildEntity Guild { get; set; }

        #endregion -- Mappings --

        #region -- DTO --

        public DTO ToDTO() => new(this);

        public class DTO
        {
            public Guid ID { get; set; }
            public string Tag { get; set; }
            public int Count { get; set; }
            public DateTime TimeFirstOccurrence { get; set; }
            public DateTime TimeLastOccurrence { get; set; }

            public DTO(GuildEventEntity entity)
            {
                ID = entity.ID;
                Tag = entity.Tag.ToString();
                Count = entity.Count;
                TimeFirstOccurrence = entity.TimeFirstOccurrence;
                TimeLastOccurrence = entity.TimeLastOccurrence;
            }
        }

        #endregion -- DTO --

    }
}
