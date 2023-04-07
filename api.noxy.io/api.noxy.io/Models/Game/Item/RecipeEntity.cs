using api.noxy.io.Models.Game.Guild;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.noxy.io.Models.Game.Item
{
    [Table("Recipe")]
    public class RecipeEntity : SingleEntity
    {
        [Required]
        [Comment("The resulting item from performing this recipe.")]
        public required ItemEntity Item { get; set; }

        [Comment("The list of items required to perform this recipe.")]
        public List<RecipeItemEntity> RecipeItemList { get; set; } = new();

        #region -- DTO --

        new public DTO ToDTO() => new(this);

        new public class DTO : SingleEntity.DTO
        {
            public ItemEntity.DTO Item  { get; set; }
            public IEnumerable<RecipeItemEntity.DTO> RecipeItemList { get; set; }

            public DTO(RecipeEntity entity) : base(entity)
            {
                Item = entity.Item.ToDTO();
                RecipeItemList = entity.RecipeItemList.Select(x => x.ToDTO());
            }
        }

        #endregion -- DTO --

    }
}
