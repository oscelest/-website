using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.noxy.io.Models.Game.Guild
{
    [Table("Feat")]
    [Index(nameof(Name), IsUnique = true)]
    public class FeatEntity : SimpleEntity
    {
        [MinLength(3), MaxLength(64)]
        public string Name { get; set; } = string.Empty;

        // Mappings
        public List<FeatRequirementEntity> FeatRequirementList { get; set; } = new();
        public List<GuildModifierEntity> GuildModifierList { get; set; } = new();

        // Inverse
        public List<RoleEntity> RoleList { get; set; } = new();
        public List<GuildEntity> GuildList { get; set; } = new();

        #region -- DTO --

        new public DTO ToDTO() => new(this);

        new public class DTO : SimpleEntity.DTO
        {
            public string Name { get; set; }
            public IEnumerable<GuildModifierEntity.DTO> GuildModifierList { get; set; }
                
            public DTO(FeatEntity entity) : base(entity)
            {
                Name = entity.Name;
                GuildModifierList = entity.GuildModifierList.Select(x => x.ToDTO());
            }
        }

        #endregion -- DTO --

    }
}
