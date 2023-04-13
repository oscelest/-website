using api.noxy.io.Utilities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.noxy.io.Models.RPG
{
    [Table("ModifierGuildMission")]
    public class ModifierGuildMission : ModifierGuild
    {
        [Required]
        [Column(nameof(MissionTag), TypeName = "varchar(32)")]
        public required ModifierGuildMissionTagType MissionTag { get; set; }
    }
}
