using api.noxy.io.Models.Game.Item;
using System.ComponentModel.DataAnnotations;

namespace api.noxy.io.Models.Game.Guild
{
    public abstract class GuildItemEntity<T> : JunctionEntity where T : ItemEntity
    {
        [Required]
        public required GuildEntity Guild { get; set; }

        public required T Item { get; set; }

        #region -- DTO --

        new public DTO ToDTO() => new(this);

        new public class DTO : JunctionEntity.DTO
        {
            public ItemEntity.DTO Item { get; set; }

            public DTO(GuildItemEntity<T> entity) : base(entity)
            {
                Item = entity.Item.ToDTO();
            }
        }

        #endregion -- DTO --
    }
}
