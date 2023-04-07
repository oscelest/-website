using api.noxy.io.Models.Game.Guild;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.noxy.io.Models.Game.Item
{
    [Index(nameof(Name), IsUnique = true)]
    public abstract class ItemEntity : SingleEntity
    {
        [Required]
        [MinLength(3), MaxLength(64)]
        public required string Name { get; set; }

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
