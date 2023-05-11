using Database.Models.RPG;
using Test.Model.Abstract;
using TestProject;

namespace Test.Model
{
    public class SlotSupportGenerator : Generator<SlotSupport>
    {
        public List<Guid> GeneratedID { get => Generated.Select(x => x.ID).ToList(); }
      
        public SlotSupport Generate(TemplateSlot slot)
        {
            SlotSupport item = TestService.Context.SlotSupport.Add(new() { Name = $"Slot Support #{++Counter}", TemplateSlot = slot }).Entity;
            Generated.Add(item);
            return item;
        }

        public List<SlotSupport> Generate(TemplateSlot slot, int total)
        {
            return new SlotSupport[total].Select(x => Generate(slot)).ToList();
        }
    }
}
