using Database.Models.RPG.Abstract;

namespace Database.Models.RPG
{
    public class TemplateFeat : Template
    {
        public List<ModifierFeat> ModifierFeatList { get; set; } = new();
        
        public List<TemplateRequirement> TemplateRequirementList { get; set; } = new();

        #region -- Mappings --

        public List<UnlockableFeat> UnlockableFeatList { get; set; } = new();

        #endregion -- Mappings --
    }
}
