using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace api.noxy.io.Models
{
    [Index(nameof(TimeCreated), AllDescending = true)]
    public abstract class Entity
    {
        [Key()]
        public Guid ID { get; set; } = Guid.NewGuid();

        [Required]
        public DateTime TimeCreated { get; set; } = DateTime.UtcNow;

        #region -- DTO --

        public DTO ToDTO() => new(this);

        public class DTO
        {
            public Guid ID { get; set; }
            public DateTime TimeCreated { get; set; }

            public DTO(Entity entity)
            {
                ID = entity.ID;
                TimeCreated = entity.TimeCreated;
            }
        }

        #endregion -- DTO --

    }
}
