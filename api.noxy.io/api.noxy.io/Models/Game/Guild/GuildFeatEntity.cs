using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.noxy.io.Models.Game.Guild
{
    [Table("AGuildHasFeat")]
    public class GuildFeatEntity : JunctionEntity
    {
        [Required]
        public required GuildEntity Guild { get; set; }

        [Required]
        public required FeatEntity Feat { get; set; }

        #region -- DTO --

        new public DTO ToDTO() => new(this);

        new public class DTO : JunctionEntity.DTO
        {
            public FeatEntity.DTO Feat { get; set; }

            public DTO(GuildFeatEntity entity) : base(entity)
            {
                Feat = entity.Feat.ToDTO();
            }
        }

        #endregion -- DTO --
    }
}
