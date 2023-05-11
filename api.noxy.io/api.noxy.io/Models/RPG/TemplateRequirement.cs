using Database.Models.RPG.Abstract;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Models.RPG
{
    [Table("TemplateRequirement")]
    public class TemplateRequirement : Template
    {
        public List<TemplateFeat> TemplateFeatList { get; set; } = new();

        public List<TemplateRole> TemplateRoleList { get; set; } = new();
    }
}
