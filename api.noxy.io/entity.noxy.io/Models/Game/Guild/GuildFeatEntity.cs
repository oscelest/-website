using System.ComponentModel.DataAnnotations.Schema;

namespace api.noxy.io.Models.Game.Guild
{
    [Table("GuildFeat")]
    public class GuildFeatEntity : JunctionEntity
    {
        public required GuildEntity Guild { get; set; } 
        public required Guid GuildID { get; set; } 

        public required FeatEntity Feat { get; set; }
        public required Guid FeatID { get; set; }

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
