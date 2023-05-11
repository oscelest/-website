using Database.Models.RPG;
using Test.Model.Abstract;
using TestProject;

namespace Test.Model
{
    public class UnitGenerator : Generator<Unit>
    {
        public Unit Generate(Guild entityGuild, List<SlotGear> listSlotGear)
        {
            TemplateUnit template = TestService.Context.TemplateUnit.Add(new() { Name = $"Template Unit #{++Counter}", SlotGearList = listSlotGear }).Entity;
            Unit item = TestService.Context.Unit.Add(new() { Guild = entityGuild, TemplateUnit = template }).Entity;
            Generated.Add(item);
            return item;
        }

        public List<Unit> Generate(Guild entityGuild, List<SlotGear> listSlotGear, int total)
        {
            return new Unit[total].Select(x => Generate(entityGuild, listSlotGear)).ToList();
        }
    }
}
