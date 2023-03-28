using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.noxy.io.Models.Game.Guild
{
    [Table("MissionType")]
    [Index(nameof(Name), IsUnique = true)]
    public class MissionTypeEntity : SimpleEntity
    {
        [Required]
        [MinLength(3), MaxLength(64)]
        public string Name { get; set; } = string.Empty;

        // Inverse
        public List<MissionEntity> MissionList { get; set; } = new();

        #region -- DTO --

        new public DTO ToDTO() => new(this);

        new public class DTO : SimpleEntity.DTO
        {
            public string Name { get; set; }

            public DTO(MissionTypeEntity entity) : base(entity)
            {
                Name = entity.Name;
            }
        }

        #endregion -- DTO --

    }
}
