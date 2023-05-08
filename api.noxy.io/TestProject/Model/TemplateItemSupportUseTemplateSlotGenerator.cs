using api.noxy.io.Models.RPG;
using Database.Models.RPG;
using Database.Models.RPG.Junction;
using Test.Model.Abstract;
using TestProject;

namespace Test.Model
{
    public class TemplateItemSupportUseTemplateSlotGenerator : Generator<TemplateItemSupportUseTemplateSlot>
    {
        public List<Guid> GeneratedID { get => Generated.Select(x => x.ID).ToList(); }

        public TemplateItemSupportUseTemplateSlot Generate(TemplateItemSupport entityTemplateItemSupport, TemplateSlot entityTemplateSlot)
        {
            TemplateItemSupportUseTemplateSlot item = TestService.Context.TemplateItemSupportUseTemplateSlot.Add(new() { TemplateItemSupport = entityTemplateItemSupport, TemplateSlot = entityTemplateSlot }).Entity;
            Generated.Add(item);
            return item;
        }

        public List<TemplateItemSupportUseTemplateSlot> Generate(TemplateItemSupport entityTemplateItemSupport, List<TemplateSlot> entityTemplateSlot)
        {
            return entityTemplateSlot.Select(x => Generate(entityTemplateItemSupport, x)).ToList();
        }

        public List<TemplateItemSupportUseTemplateSlot> Generate(TemplateItemSupport entityTemplateItemSupport, TemplateSlot entityTemplateSlot, int total)
        {
            return new TemplateItemSupportUseTemplateSlot[total].Select(x => Generate(entityTemplateItemSupport, entityTemplateSlot)).ToList();
        }
    }
}
