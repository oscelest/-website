using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.noxy.io.Models.Game.Guild
{
    [Table("Feat")]
    [Index(nameof(Name), IsUnique = true)]
    public class FeatEntity : SingleEntity
    {
        [Required]
        [MinLength(3), MaxLength(64)]
        public required string Name { get; set; } = string.Empty;

        public required List<RequirementEntity> RequirementList { get; set; } = new();

        public required List<GuildModifierEntity> GuildModifierList { get; set; } = new();

        #region -- Mapping --

        public List<GuildFeatEntity> GuildFeatList { get; set; } = new();

        #endregion -- Mapping --

        #region -- DTO --

        new public DTO ToDTO() => new(this);

        new public class DTO : SingleEntity.DTO
        {
            public string Name { get; set; }
            public IEnumerable<GuildModifierEntity.DTO> GuildModifierList { get; set; }
            public IEnumerable<RequirementEntity.DTO> RequirementList { get; set; }

            public DTO(FeatEntity entity) : base(entity)
            {
                Name = entity.Name;
                GuildModifierList = entity.GuildModifierList.Select(x => x.ToDTO());
                RequirementList = entity.RequirementList.Select(x => x.ToDTO());
            }
        }

        #endregion -- DTO --

    }
}
