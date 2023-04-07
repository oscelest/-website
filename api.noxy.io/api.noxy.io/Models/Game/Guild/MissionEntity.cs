﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.noxy.io.Models.Game.Guild
{
    [Table("Mission")]
    public class MissionEntity : SingleEntity
    {
        public DateTime? TimeStarted { get; set; } = default;

        [Required]
        public required GuildEntity Guild { get; set; }

        #region -- Mapping --

        public List<UnitEntity> UnitList { get; set; } = new();

        public List<GuildRoleEntity> RoleList { get; set; } = new();

        #endregion -- Mapping --

        #region -- DTO --

        new public DTO ToDTO() => new(this);

        new public class DTO : SingleEntity.DTO
        {
            public IEnumerable<UnitEntity.DTO> UnitList { get; set; }
            public IEnumerable<GuildRoleEntity.DTO> RoleList { get; set; }
            public DateTime? TimeStarted { get; set; }

            public DTO(MissionEntity entity) : base(entity)
            {
                UnitList = entity.UnitList.Select(x => x.ToDTO());
                RoleList = entity.RoleList.Select(x => x.ToDTO());
                TimeStarted = entity.TimeStarted;
            }
        }

        #endregion -- DTO --

    }
}
