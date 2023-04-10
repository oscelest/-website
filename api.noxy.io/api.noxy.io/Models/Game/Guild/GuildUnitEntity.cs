using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.noxy.io.Models.Game.Guild
{
    [Table("GuildUnit")]
    public class GuildUnitEntity : JunctionEntity
    {
        [Required]
        public required GuildEntity Guild { get; set; }

        [Required]
        public required UnitEntity Unit { get; set; }

        #region -- DTO --

        new public DTO ToDTO() => new(this);

        new public class DTO : JunctionEntity.DTO
        {
            public UnitEntity.DTO Unit { get; set; }

            public DTO(GuildUnitEntity entity) : base(entity)
            {
                Unit = entity.Unit.ToDTO();
            }
        }

        #endregion -- DTO --

    }
}
