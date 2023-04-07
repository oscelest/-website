using System.ComponentModel.DataAnnotations;

namespace api.noxy.io.Models.Game.Item
{
    public class ItemSocketEntity : JunctionEntity
    {
        [Required]
        public required ItemEntity Item { get; set; }

        [Required]
        public required SocketEntity Socket { get; set; }

        #region -- DTO --

        new public DTO ToDTO() => new(this);

        new public class DTO : JunctionEntity.DTO
        {
            public ItemEntity Item { get; set; }
            public SocketEntity Socket { get; set; }

            public DTO(ItemSocketEntity entity) : base(entity)
            {
                Item = entity.Item;
                Socket = entity.Socket;
            }
        }

        #endregion -- DTO --

    }
}
