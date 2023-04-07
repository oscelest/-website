using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.noxy.io.Models.Game.Item
{
    [Index(nameof(Name), IsUnique = true)]
    public class SocketEntity : SingleEntity
    {
        [Required]
        [MinLength(3), MaxLength(64)]
        public required string Name { get; set; }

        [Required]
        public required string Description { get; set; }

        #region -- Mapping --

        public List<ItemSocketEntity> ItemSocketList { get; set; } = new List<ItemSocketEntity>();

        #endregion -- Mapping --

        #region -- DTO --

        new public DTO ToDTO() => new(this);

        new public class DTO : SingleEntity.DTO
        {
            public string Name { get; set; }

            public DTO(ItemEntity entity) : base(entity)
            {
                Name = entity.Name;
            }
        }

        #endregion -- DTO --

    }
}
