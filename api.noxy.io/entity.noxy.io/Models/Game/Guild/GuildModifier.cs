using api.noxy.io.Utilities;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.noxy.io.Models.Game.Guild
{
    public abstract class GuildModifierEntity : SingleEntity
    {
        [Required]
        public float Value { get; set; }

        [Required]
        [Column(TypeName = "varchar(32)")]
        public ArithmeticalTagType ArithmeticalTag { get; set; }

        public required FeatEntity Feat { get; set; }
        public required Guid FeatID { get; set; }

        #region -- DTO --

        new public DTO ToDTO() => new(this);

        new public class DTO : SingleEntity.DTO
        {
            public float Value { get; set; }
            public ArithmeticalTagType ArithmeticalTag { get; set; }

            public DTO(GuildModifierEntity entity) : base(entity)
            {
                Value = entity.Value;
                ArithmeticalTag = entity.ArithmeticalTag;
            }
        }

        #endregion -- DTO --

    }
}
