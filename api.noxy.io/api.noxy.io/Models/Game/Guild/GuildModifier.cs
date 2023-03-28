using api.noxy.io.Utilities;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.noxy.io.Models.Game.Guild
{
    public abstract class GuildModifierEntity : SimpleEntity
    {
        [Required]
        public float Value { get; set; }

        [Required]
        [Column(TypeName = "varchar(32)")]
        public ArithmeticalTagType ArithmeticalTag { get; set; }

        // Inverse
        public FeatEntity Feat { get; set; } = new();

        #region -- DTO --

        new public DTO ToDTO() => new(this);

        new public class DTO : SimpleEntity.DTO
        {
            public float Value { get; set; }
            public ArithmeticalTagType ArithmeticalTag { get; set; }

            public DTO(GuildModifierEntity entity) : base(entity)
            {
                //EntityType = entity.EntityType;
                Value = entity.Value;
                ArithmeticalTag = entity.ArithmeticalTag;
            }
        }

        #endregion -- DTO --

    }
}
