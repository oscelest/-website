using Database.Models.RPG;
using Test.Model.Abstract;
using TestProject;

namespace Test.Model
{
    public class TemplateSlotGenerator : Generator<TemplateSlot>
    {
        public TemplateSlot Generate()
        {
            TemplateSlot item = TestService.Context.TemplateSlot.Add(new() { Name = $"Slot Gear {++Counter}" }).Entity;
            Generated.Add(item);
            return item;
        }

        public List<TemplateSlot> Generate(int total)
        {
            return new TemplateSlot[total].Select(x => Generate()).ToList();
        }
    }
}
