namespace api.noxy.io.Models.RPG
{
    public class TemplateFeat : Template
    {
        public List<TemplateRequirement> TemplateRequirementList { get; set; } = new();

        public List<ModifierGuild> ModifierGuildList { get; set; } = new();
    }
}
