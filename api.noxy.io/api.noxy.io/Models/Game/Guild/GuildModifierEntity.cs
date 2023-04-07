using api.noxy.io.Utilities;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.noxy.io.Models.Game.Guild
{
    public abstract class GuildModifierEntity : SingleEntity
    {
        [Required]
        public required float Value { get; set; }

        [Required]
        [Column(TypeName = "varchar(32)")]
        public required ArithmeticalTagType ArithmeticalTag { get; set; }

        [Required]
        public required FeatEntity Feat { get; set; }

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
