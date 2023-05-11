using Database.Models.RPG;
using Test.Model.Abstract;
using TestProject;

namespace Test.Model
{
    public class TemplateItemGearGenerator : Generator<TemplateItemGear>
    {
        public TemplateItemGear Generate()
        {
            TemplateItemGear item = TestService.Context.TemplateItemGear.Add(new() { Name = $"Template Item Gear {++Counter}" }).Entity;
            Generated.Add(item);
            return item;
        }

        public List<TemplateItemGear> Generate(int total)
        {
            return new TemplateItemGear[total].Select(x => Generate()).ToList();
        }
    }
}
