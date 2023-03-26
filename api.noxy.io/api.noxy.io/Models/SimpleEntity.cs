using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace api.noxy.io.Models
{
    [Index(nameof(TimeCreated), AllDescending = true)]
    public abstract class SimpleEntity
    {
        [Key()]
        public Guid ID { get; set; }

        [Required]
        public DateTime TimeCreated { get; set; }

        public DateTime? TimeUpdated { get; set; }

        public DTO ToDTO() => new(this);

        public class DTO
        {
            public Guid ID { get; set; }
            public DateTime TimeCreated { get; set; }
            public DateTime? TimeUpdated { get; set; }

            public DTO(SimpleEntity entity)
            {
                ID = entity.ID;
                TimeCreated = entity.TimeCreated;
                TimeUpdated = entity.TimeUpdated;
            }
        }
    }
}
