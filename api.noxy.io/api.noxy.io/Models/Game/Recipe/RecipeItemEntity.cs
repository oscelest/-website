using api.noxy.io.Models.Game.Item;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.noxy.io.Models.Game.Recipe
{
    [Table("Recipe")]
    public class RecipeItemEntity : JunctionEntity
    {

        [Required]
        [Comment("The recipe the item is needed for")]
        public required RecipeEntity Recipe { get; set; }

        [Required]
        [Comment("The specific item needed")]
        public required ItemEntity Item { get; set; }

        [Required]
        [Comment("The amount of the item that's needed")]
        public required int Count { get; set; }

        #region -- DTO --

        new public DTO ToDTO() => new(this);

        new public class DTO : JunctionEntity.DTO
        {
            public ItemEntity.DTO Item { get; set; }
            public RecipeEntity.DTO Recipe { get; set; }
            public int Count { get; set; }

            public DTO(RecipeItemEntity entity) : base(entity)
            {
                Item = entity.Item.ToDTO();
                Recipe = entity.Recipe.ToDTO();
                Count = entity.Count;
            }
        }

        #endregion -- DTO --

    }
}
