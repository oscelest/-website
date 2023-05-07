using api.noxy.io.Models.RPG;
using Database.Models.RPG;
using Database.Models.RPG.Junction;
using Test.Model.Abstract;
using TestProject;

namespace Test.Model
{
    public class TemplateItemGearUseTemplateSlotGenerator : Generator<TemplateItemGearUseTemplateSlot>
    {
        public List<Guid> GeneratedID { get => Generated.Select(x => x.ID).ToList(); }

        public TemplateItemGearUseTemplateSlot Generate(TemplateItemGear entityTemplateItemGear, TemplateSlot entityTemplateSlot)
        {
            TemplateItemGearUseTemplateSlot item = TestService.Context.TemplateItemGearUseTemplateSlot.Add(new() { TemplateItemGear = entityTemplateItemGear, TemplateSlot = entityTemplateSlot }).Entity;
            Generated.Add(item);
            return item;
        }

        public List<TemplateItemGearUseTemplateSlot> Generate(TemplateItemGear entityTemplateItemGear, List<TemplateSlot> entityTemplateSlot)
        {
            return entityTemplateSlot.Select(x => Generate(entityTemplateItemGear, x)).ToList();
        }

        public List<TemplateItemGearUseTemplateSlot> Generate(TemplateItemGear entityTemplateItemGear, TemplateSlot entityTemplateSlot, int total)
        {
            return new TemplateItemGearUseTemplateSlot[total].Select(x => Generate(entityTemplateItemGear, entityTemplateSlot)).ToList();
        }
    }
}
