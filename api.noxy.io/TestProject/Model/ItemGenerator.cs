using api.noxy.io.Models.RPG;
using Database.Models.RPG;
using Test.Model.Abstract;
using TestProject;

namespace Test.Model
{
    public class ItemGenerator : Generator<Item>
    {
        public Item GenerateGear(Guild guild, List<SlotSupport> listSlotSupport, int count = 1)
        {
            TemplateItem template = TestService.Context.TemplateItemGear.Add(new() { Name = $"Template Item Gear #{++Counter}", SlotSupportList = listSlotSupport }).Entity;
            Item item = TestService.Context.Item.Add(new() { Guild = guild, TemplateItem = template, Count = count }).Entity;
            Generated.Add(item);
            return item;
        }

        public List<Item> GenerateGear(Guild guild, List<SlotSupport> listSlotSupport, int count, int total)
        {
            return new Item[total].Select(x => GenerateGear(guild, listSlotSupport, count)).ToList();
        }

        public Item GenerateSupport(Guild guild, int count = 1)
        {
            TemplateItem template = TestService.Context.TemplateItemSupport.Add(new() { Name = $"Template Item Support #{++Counter}",  }).Entity;
            Item item = TestService.Context.Item.Add(new() { Guild = guild, TemplateItem = template, Count = count }).Entity;
            Generated.Add(item);
            return item;
        }

        public List<Item> GenerateSupport(Guild guild, int count, int total)
        {
            return new Item[total].Select(x => GenerateSupport(guild, count)).ToList();
        }
    }
}
