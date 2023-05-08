using api.noxy.io.Models.RPG;
using Database.Models.RPG;
using Test.Model.Abstract;
using TestProject;

namespace Test.Model
{
    public class SlotGearGenerator : Generator<SlotGear>
    {
        public List<Guid> GeneratedID { get => Generated.Select(x => x.ID).ToList(); }
      
        public SlotGear Generate(TemplateSlot slot)
        {
            SlotGear item = TestService.Context.SlotGear.Add(new() { Name = $"Slot Gear #{++Counter}", TemplateSlot = slot }).Entity;
            Generated.Add(item);
            return item;
        }

        public List<SlotGear> Generate(TemplateSlot slot, int total)
        {
            return new SlotGear[total].Select(x => Generate(slot)).ToList();
        }
    }
}
