using TestProject;
using Test.Model;
using Database.Models.RPG;

namespace Test.RPGRepository
{
    [TestClass]
    public class EquipSupportTest
    {
        [TestMethod]
        public async Task EquipSupport_Basic_Success()
        {
            Guild entityGuild = TestService.GetNewGuild();
            Generator generator = new();

            generator.TemplateSlot.Generate(2);
            generator.SlotGear.Generate(generator.TemplateSlot[0]);
            generator.SlotSupport.Generate(generator.TemplateSlot[1]);
            generator.Unit.Generate(entityGuild, new() { generator.SlotGear.Current });
            generator.Item.GenerateGear(entityGuild, new() { generator.SlotSupport.Current });
            generator.Item.GenerateSupport(entityGuild);
            generator.TemplateItemGearSlot.Generate(generator.Item[0].TemplateItemGear, generator.TemplateSlot[0]);
            generator.TemplateItemSupportSlot.Generate(generator.Item[1].TemplateItemSupport, generator.TemplateSlot[1]);

            TestService.Context.SaveChanges();

            EquipmentGear entityEquipmentGear = await TestService.RPGRepository.EquipGear(entityGuild, generator.Unit.Current.ID, generator.Item[0].ID, generator.SlotGear.GeneratedID);
            await TestService.RPGRepository.EquipSupport(entityGuild, entityEquipmentGear.ID, generator.Item[1].ID, generator.SlotSupport.GeneratedID);
        }

        private class Generator
        {
            public ItemGenerator Item { get; set; } = new();
            public UnitGenerator Unit { get; set; } = new();
            public SlotGearGenerator SlotGear { get; set; } = new();
            public SlotSupportGenerator SlotSupport { get; set; } = new();
            public TemplateSlotGenerator TemplateSlot { get; set; } = new();
            public TemplateItemGearUseTemplateSlotGenerator TemplateItemGearSlot { get; set; } = new();
            public TemplateItemSupportUseTemplateSlotGenerator TemplateItemSupportSlot { get; set; } = new();
        }
    }
}
