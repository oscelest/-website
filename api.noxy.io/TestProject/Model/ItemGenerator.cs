using api.noxy.io.Models.RPG;
using Database.Models.RPG;
using Test.Model.Abstract;
using TestProject;

namespace Test.Model
{
    public class ItemGenerator : Generator<Item>
    {
        public Item GenerateGear(Guild guild, int count = 1)
        {
            TemplateItem template = TestService.Context.TemplateItemGear.Add(new() { Name = $"Template Item #{++Counter}" }).Entity;
            Item item = TestService.Context.Item.Add(new() { Guild = guild, TemplateItem = template, Count = count }).Entity;
            Generated.Add(item);
            return item;
        }

        public List<Item> GenerateGear(Guild guild, int count, int total)
        {
            return new Item[total].Select(x => GenerateGear(guild, count)).ToList();
        }
    }
}
