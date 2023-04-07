using api.noxy.io.Models.Game.Item;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.noxy.io.Models.Game.Guild
{
    [Table("GuildItem")]
    public class GuildItemEntity : SingleEntity
    {
        [Required]
        public required int Count { get; set; }

        [Required]
        public required GuildEntity Guild { get; set; }

        [Required]
        public required ItemEntity Item { get; set; }

        #region -- DTO --

        new public DTO ToDTO() => new(this);

        new public class DTO : JunctionEntity.DTO
        {
            public int Count { get; set; }
            public ItemEntity.DTO Item { get; set; }

            public DTO(GuildItemEntity entity) : base(entity)
            {
                Count = entity.Count;
                Item = entity.Item.ToDTO();
            }
        }

        #endregion -- DTO --
    }
}
