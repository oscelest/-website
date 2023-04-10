using api.noxy.io.Models.Game.Item;
using System.ComponentModel.DataAnnotations;

namespace api.noxy.io.Models.Game.Guild
{
    public class GuildMaterialItemEntity : GuildItemEntity<MaterialItemEntity>
    {
        [Required]
        public required int Count { get; set; }

        #region -- DTO --

        new public DTO ToDTO() => new(this);

        new public class DTO : GuildItemEntity<MaterialItemEntity>.DTO
        {
            public int Count { get; set; }
            new public MaterialItemEntity.DTO Item { get; set; }

            public DTO(GuildMaterialItemEntity entity) : base(entity)
            {
                Count = entity.Count;
                Item = entity.Item.ToDTO();
            }
        }

        #endregion -- DTO --
    }
}
