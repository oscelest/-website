using Database.Models.RPG.Abstract;
using Database.Utilities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Models.RPG
{
    [Table("ModifierGuildMission")]
    public class ModifierGuildMission : ModifierFeat
    {
        [Required]
        [Column(nameof(MissionTag), TypeName = "varchar(32)")]
        public required ModifierGuildMissionTagType MissionTag { get; set; }
    }
}
