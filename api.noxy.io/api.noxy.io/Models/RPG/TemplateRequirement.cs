using System.ComponentModel.DataAnnotations.Schema;

namespace api.noxy.io.Models.RPG
{
    [Table("TemplateRequirement")]
    public class TemplateRequirement : Template
    {
        public List<TemplateFeat> TemplateFeatList { get; set; } = new();

        public List<TemplateRole> TemplateRoleList { get; set; } = new();
    }
}
