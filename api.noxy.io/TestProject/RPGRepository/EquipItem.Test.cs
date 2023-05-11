using Database.Models.RPG.Junction;
using Database.Models.RPG;
using TestProject;
using Microsoft.EntityFrameworkCore;
using Test.Model;
using Database.Exceptions;

namespace Test.RPGRepository
{
    [TestClass]
    public class EquipItemTest
    {
        [TestMethod]
        public async Task EquipItem_UnitEmpty_Failure()
        {
            Guild entityGuild = TestService.GetNewGuild();
            Generator generator = new();

            generator.Item.GenerateGear(entityGuild, new() { });
            generator.TemplateSlot.Generate();
            generator.SlotGear.Generate(generator.TemplateSlot.Current);
            generator.Unit.Generate(entityGuild, generator.SlotGear.Generated);
            generator.TemplateItemGearSlot.Generate(generator.Item.Current.TemplateItemGear, generator.TemplateSlot.Current);

            TestService.Context.SaveChanges();

            try
            {
                await TestService.RPGRepository.EquipGear(entityGuild, Guid.Empty, generator.Item.Current.ID, generator.SlotGear.GeneratedID);
                Assert.Fail("Call to EquipGear should fail.");
            }
            catch (EntityNotFoundException<Unit> ex)
            {
                Type type = ex.Identifier.GetType();
                Assert.AreEqual(entityGuild.ID, (Guid?)type.GetProperty("Guild")?.GetValue(ex.Identifier, null));
                Assert.AreEqual(Guid.Empty, (Guid?)type.GetProperty("Unit")?.GetValue(ex.Identifier, null));
            }
        }

        [TestMethod]
        public async Task EquipItem_ItemEmpty_Failure()
        {
            Guild entityGuild = TestService.GetNewGuild();
            Generator generator = new();

            generator.Item.GenerateGear(entityGuild, new() { });
            generator.TemplateSlot.Generate();
            generator.SlotGear.Generate(generator.TemplateSlot.Current);
            generator.Unit.Generate(entityGuild, generator.SlotGear.Generated);
            generator.TemplateItemGearSlot.Generate(generator.Item.Current.TemplateItemGear, generator.TemplateSlot.Current);

            TestService.Context.SaveChanges();

            try
            {
                await TestService.RPGRepository.EquipGear(entityGuild, generator.Unit.Current.ID, Guid.Empty, generator.SlotGear.GeneratedID);
                Assert.Fail("Call to EquipGear should fail.");
            }
            catch (EntityNotFoundException<Item> ex)
            {
                Type type = ex.Identifier.GetType();
                Assert.AreEqual(entityGuild.ID, (Guid?)type.GetProperty("Guild")?.GetValue(ex.Identifier, null));
                Assert.AreEqual(Guid.Empty, (Guid?)type.GetProperty("Item")?.GetValue(ex.Identifier, null));
            }
        }

        [TestMethod]
        public async Task EquipItem_SlotEmpty_Failure()
        {
            Guild entityGuild = TestService.GetNewGuild();
            Generator generator = new();

            generator.Item.GenerateGear(entityGuild, new() { });
            generator.TemplateSlot.Generate();
            generator.SlotGear.Generate(generator.TemplateSlot.Current);
            generator.Unit.Generate(entityGuild, generator.SlotGear.Generated);
            generator.TemplateItemGearSlot.Generate(generator.Item.Current.TemplateItemGear, generator.TemplateSlot.Current);

            TestService.Context.SaveChanges();

            try
            {
                await TestService.RPGRepository.EquipGear(entityGuild, generator.Unit.Current.ID, generator.Item.Current.ID, new() { Guid.Empty });
                Assert.Fail("Call to EquipGear should fail.");
            }
            catch (EntityNotFoundException<TemplateItemGearUseTemplateSlot> ex)
            {
                List<Guid> list = (List<Guid>)ex.Identifier;
                Assert.IsNotNull(list);
                Assert.AreEqual(1, list.Count);
                Assert.IsTrue(list.Any(generator.TemplateItemGearSlot.GeneratedID.Contains));
            }
        }

        [TestMethod]
        public async Task EquipItem_ItemCountZero_Failure()
        {
            Guild entityGuild = TestService.GetNewGuild();
            Generator generator = new();

            generator.Item.GenerateGear(entityGuild, new() { }, 0);
            generator.TemplateSlot.Generate();
            generator.SlotGear.Generate(generator.TemplateSlot.Current);
            generator.Unit.Generate(entityGuild, generator.SlotGear.Generated);
            generator.TemplateItemGearSlot.Generate(generator.Item.Current.TemplateItemGear, generator.TemplateSlot.Current);

            TestService.Context.SaveChanges();

            try
            {
                await TestService.RPGRepository.EquipGear(entityGuild, generator.Unit.Current.ID, generator.Item.Current.ID, generator.SlotGear.GeneratedID);
                Assert.Fail("Call to EquipGear should fail.");
            }
            catch (EntityNotFoundException<Item> ex)
            {
                Type type = ex.Identifier.GetType();
                Assert.AreEqual(entityGuild.ID, (Guid?)type.GetProperty("Guild")?.GetValue(ex.Identifier, null));
                Assert.AreEqual(generator.Item.Current.ID, (Guid?)type.GetProperty("Item")?.GetValue(ex.Identifier, null));
            }
        }

        [TestMethod]
        public async Task EquipItem_UnitHasNoSlot_Failure()
        {
            Guild entityGuild = TestService.GetNewGuild();
            Generator generator = new();

            generator.Item.GenerateGear(entityGuild, new() { });
            generator.TemplateSlot.Generate();
            generator.SlotGear.Generate(generator.TemplateSlot.Current);
            generator.Unit.Generate(entityGuild, new() { });
            generator.TemplateItemGearSlot.Generate(generator.Item.Current.TemplateItemGear, generator.TemplateSlot.Current);

            TestService.Context.SaveChanges();

            try
            {
                await TestService.RPGRepository.EquipGear(entityGuild, generator.Unit.Current.ID, generator.Item.Current.ID, generator.SlotGear.GeneratedID);
                Assert.Fail("Call to EquipGear should fail.");
            }
            catch (EntityNotFoundException<TemplateItemGearUseTemplateSlot> ex)
            {
                List<Guid> list = (List<Guid>)ex.Identifier;
                Assert.IsNotNull(list);
                Assert.AreEqual(1, list.Count);
                Assert.IsTrue(list.Any(generator.TemplateItemGearSlot.GeneratedID.Contains));
            }
        }

        [TestMethod]
        public async Task EquipItem_NoTemplateItemGearUseTemplateSlot_Failure()
        {
            Guild entityGuild = TestService.GetNewGuild();
            Generator generator = new();

            generator.Item.GenerateGear(entityGuild, new() { });
            generator.TemplateSlot.Generate();
            generator.SlotGear.Generate(generator.TemplateSlot.Current);
            generator.Unit.Generate(entityGuild, generator.SlotGear.Generated);

            TestService.Context.SaveChanges();

            try
            {
                await TestService.RPGRepository.EquipGear(entityGuild, generator.Unit.Current.ID, generator.Item.Current.ID, generator.SlotGear.GeneratedID);
                Assert.Fail("Call to EquipGear should fail.");
            }
            catch (EntityOperationException<TemplateItemGear>)
            {
                // Success
            }
        }

        [TestMethod]
        public async Task EquipItem_SameTemplateDifferentSlot_Failure()
        {
            Guild entityGuild = TestService.GetNewGuild();
            Generator generator = new();

            generator.Item.GenerateGear(entityGuild, new() { });
            generator.TemplateSlot.Generate();
            generator.SlotGear.Generate(generator.TemplateSlot.Current, 2);
            generator.Unit.Generate(entityGuild, new() { generator.SlotGear[0] });
            generator.TemplateItemGearSlot.Generate(generator.Item.Current.TemplateItemGear, generator.TemplateSlot.Current);

            TestService.Context.SaveChanges();

            try
            {
                await TestService.RPGRepository.EquipGear(entityGuild, generator.Unit.Current.ID, generator.Item.Current.ID, new() { generator.SlotGear[1].ID });
                Assert.Fail("Call to EquipGear should fail.");
            }
            catch (EntityNotFoundException<TemplateItemGearUseTemplateSlot> ex)
            {
                List<Guid> list = (List<Guid>)ex.Identifier;
                Assert.IsNotNull(list);
                Assert.AreEqual(1, list.Count);
                Assert.IsTrue(list.Any(generator.TemplateItemGearSlot.GeneratedID.Contains));
            }
        }

        [TestMethod]
        public async Task EquipItem_NotEnoughSlotGearGiven_Failure()
        {
            Guild entityGuild = TestService.GetNewGuild();
            Generator generator = new();

            generator.Item.GenerateGear(entityGuild, new() { });
            generator.TemplateSlot.Generate();
            generator.SlotGear.Generate(generator.TemplateSlot.Current);
            generator.Unit.Generate(entityGuild, generator.SlotGear.Generated);
            generator.TemplateItemGearSlot.Generate(generator.Item.Current.TemplateItemGear, generator.TemplateSlot.Current);

            TestService.Context.SaveChanges();

            try
            {
                await TestService.RPGRepository.EquipGear(entityGuild, generator.Unit.Current.ID, generator.Item.Current.ID, new() { });
                Assert.Fail("Call to EquipGear should fail.");
            }
            catch (EntityNotFoundException<TemplateItemGearUseTemplateSlot> ex)
            {
                List<Guid> list = (List<Guid>)ex.Identifier;
                Assert.IsNotNull(list);
                Assert.AreEqual(1, list.Count);
                Assert.IsTrue(list.Any(generator.TemplateItemGearSlot.GeneratedID.Contains));
            }
        }

        [TestMethod]
        public async Task EquipItem_RequireMoreGearSlotThanUnitHas_Failure()
        {
            Guild entityGuild = TestService.GetNewGuild();
            Generator generator = new();

            generator.Item.GenerateGear(entityGuild, new() { });
            generator.TemplateSlot.Generate();
            generator.SlotGear.Generate(generator.TemplateSlot.Current, 2);
            generator.Unit.Generate(entityGuild, new List<SlotGear>() { generator.SlotGear.Current });
            generator.TemplateItemGearSlot.Generate(generator.Item.Current.TemplateItemGear, generator.TemplateSlot.Current, 2);

            TestService.Context.SaveChanges();

            try
            {
                await TestService.RPGRepository.EquipGear(entityGuild, generator.Unit.Current.ID, generator.Item.Current.ID, generator.SlotGear.GeneratedID);
                Assert.Fail("Call to EquipGear should fail.");
            }
            catch (EntityNotFoundException<TemplateItemGearUseTemplateSlot> ex)
            {
                List<Guid> list = (List<Guid>)ex.Identifier;
                Assert.IsNotNull(list);
                Assert.AreEqual(1, list.Count);
                Assert.IsTrue(list.Any(generator.TemplateItemGearSlot.GeneratedID.Contains));
            }
        }

        [TestMethod]
        public async Task EquipItem_UseSameGearSlotTwice_Failure()
        {
            Guild entityGuild = TestService.GetNewGuild();
            Generator generator = new();

            generator.Item.GenerateGear(entityGuild, new() { });
            generator.TemplateSlot.Generate();
            generator.SlotGear.Generate(generator.TemplateSlot.Current);
            generator.Unit.Generate(entityGuild, generator.SlotGear.Generated);
            generator.TemplateItemGearSlot.Generate(generator.Item.Current.TemplateItemGear, generator.TemplateSlot.Current, 2);

            TestService.Context.SaveChanges();

            try
            {
                await TestService.RPGRepository.EquipGear(entityGuild, generator.Unit.Current.ID, generator.Item.Current.ID, new() { generator.SlotGear.Current.ID, generator.SlotGear.Current.ID });
                Assert.Fail("Call to EquipGear should fail.");
            }
            catch (EntityNotFoundException<TemplateItemGearUseTemplateSlot> ex)
            {
                List<Guid> list = (List<Guid>)ex.Identifier;
                Assert.IsNotNull(list);
                Assert.AreEqual(1, list.Count);
                Assert.IsTrue(list.Any(generator.TemplateItemGearSlot.GeneratedID.Contains));
            }
        }


        [TestMethod]
        public async Task EquipItem_Basic_Success()
        {
            Guild entityGuild = TestService.GetNewGuild();
            Generator generator = new();

            generator.Item.GenerateGear(entityGuild, new() { });
            generator.TemplateSlot.Generate();
            generator.SlotGear.Generate(generator.TemplateSlot.Current);
            generator.Unit.Generate(entityGuild, generator.SlotGear.Generated);
            generator.TemplateItemGearSlot.Generate(generator.Item.Current.TemplateItemGear, generator.TemplateSlot.Generated);

            TestService.Context.SaveChanges();

            await TestService.RPGRepository.EquipGear(entityGuild, generator.Unit.Current.ID, generator.Item.Current.ID, generator.SlotGear.GeneratedID);

            Item? ResultItem = TestService.Context.Item.FirstOrDefault(x => x.ID == generator.Item.Current.ID);
            Assert.IsNotNull(ResultItem);
            Assert.AreEqual(0, ResultItem?.Count);

            EquipmentGear? ResultEquipmentGear = TestService.Context.EquipmentGear.FirstOrDefault(x => x.TemplateItemGear == generator.Item.Current.TemplateItemGear && x.Unit == generator.Unit.Current);
            Assert.IsNotNull(ResultEquipmentGear);

            int ResultTemplateItemGearTemplateSlot = TestService.Context.TemplateItemGearUseTemplateSlot.Count(x => x.TemplateItemGear == generator.Item.Current.TemplateItemGear && x.TemplateSlot == generator.TemplateSlot.Current);
            Assert.AreEqual(1, ResultTemplateItemGearTemplateSlot);
        }

        [TestMethod]
        public async Task EquipItem_TwoOfTheSameTemplateSlot_Success()
        {
            Guild entityGuild = TestService.GetNewGuild();
            Generator generator = new();

            generator.Item.GenerateGear(entityGuild, new() { });
            generator.TemplateSlot.Generate();
            generator.SlotGear.Generate(generator.TemplateSlot.Current, 2);
            generator.Unit.Generate(entityGuild, generator.SlotGear.Generated);
            generator.TemplateItemGearSlot.Generate(generator.Item.Current.TemplateItemGear, generator.TemplateSlot.Generated);

            TestService.Context.SaveChanges();

            await TestService.RPGRepository.EquipGear(entityGuild, generator.Unit.Current.ID, generator.Item.Current.ID, generator.SlotGear.GeneratedID);

            Item? ResultItem = TestService.Context.Item.FirstOrDefault(x => x.ID == generator.Item.Current.ID);
            Assert.IsNotNull(ResultItem);
            Assert.AreEqual(0, ResultItem?.Count);

            EquipmentGear? ResultEquipmentGear = TestService.Context.EquipmentGear.FirstOrDefault(x => x.TemplateItemGear.ID == generator.Item.Current.TemplateItem.ID && x.Unit!.ID == generator.Unit.Current.ID);
            Assert.IsNotNull(ResultEquipmentGear);
        }

        [TestMethod]
        public async Task EquipItem_EquipSameTemplateItemGearTwice_Success()
        {
            Guild entityGuild = TestService.GetNewGuild();
            Generator generator = new();

            generator.Item.GenerateGear(entityGuild, new() { }, 2);
            generator.TemplateSlot.Generate();
            generator.SlotGear.Generate(generator.TemplateSlot.Current);
            generator.Unit.Generate(entityGuild, generator.SlotGear.Generated);
            generator.TemplateItemGearSlot.Generate(generator.Item.Current.TemplateItemGear, generator.TemplateSlot.Generated);

            TestService.Context.SaveChanges();

            await TestService.RPGRepository.EquipGear(entityGuild, generator.Unit.Current.ID, generator.Item.Current.ID, generator.SlotGear.GeneratedID);
            await TestService.RPGRepository.EquipGear(entityGuild, generator.Unit.Current.ID, generator.Item.Current.ID, generator.SlotGear.GeneratedID);

            Item? ResultItem = TestService.Context.Item.FirstOrDefault(x => x.ID == generator.Item.Current.ID);
            Assert.IsNotNull(ResultItem);
            Assert.AreEqual(0, ResultItem?.Count);

            int ResultEquipmentGear1 = TestService.Context.EquipmentGear.Count(x => x.TemplateItemGear.ID == generator.Item.Current.TemplateItem.ID && x.Unit!.ID == generator.Unit.Current.ID);
            Assert.AreEqual(1, ResultEquipmentGear1);

            int ResultEquipmentGear2 = TestService.Context.EquipmentGear.Count(x => x.TemplateItemGear.ID == generator.Item.Current.TemplateItem.ID && x.Unit == null);
            Assert.AreEqual(1, ResultEquipmentGear2);
        }

        [TestMethod]
        public async Task EquipItem_TwoOfThreeSlotOverlap_Success()
        {
            Guild entityGuild = TestService.GetNewGuild();
            Generator generator = new();

            generator.Item.GenerateGear(entityGuild, new() { }, 1, 2);
            generator.TemplateSlot.Generate(3);
            generator.SlotGear.Generate(generator.TemplateSlot[0]);
            generator.SlotGear.Generate(generator.TemplateSlot[1]);
            generator.SlotGear.Generate(generator.TemplateSlot[2]);
            generator.Unit.Generate(entityGuild, generator.SlotGear.Generated);
            generator.TemplateItemGearSlot.Generate(generator.Item[0].TemplateItemGear, generator.TemplateSlot[0]);
            generator.TemplateItemGearSlot.Generate(generator.Item[0].TemplateItemGear, generator.TemplateSlot[1]);
            generator.TemplateItemGearSlot.Generate(generator.Item[1].TemplateItemGear, generator.TemplateSlot[1]);
            generator.TemplateItemGearSlot.Generate(generator.Item[1].TemplateItemGear, generator.TemplateSlot[2]);

            TestService.Context.SaveChanges();

            await TestService.RPGRepository.EquipGear(entityGuild, generator.Unit.Current.ID, generator.Item.Generated[0].ID, new() { generator.SlotGear[0].ID, generator.SlotGear[1].ID });
            await TestService.RPGRepository.EquipGear(entityGuild, generator.Unit.Current.ID, generator.Item.Generated[1].ID, new() { generator.SlotGear[1].ID, generator.SlotGear[2].ID });

            Assert.AreEqual(0, TestService.Context.Item.FirstOrDefault(x => x.ID == generator.Item[0].ID)?.Count);
            Assert.AreEqual(0, TestService.Context.Item.FirstOrDefault(x => x.ID == generator.Item[1].ID)?.Count);

            EquipmentGear? ResultEquipmentGear1 = await TestService.Context.EquipmentGear.FirstOrDefaultAsync(x => x.TemplateItemGear == generator.Item[0].TemplateItemGear && x.Unit == generator.Unit.Current);
            Assert.IsNull(ResultEquipmentGear1);

            EquipmentGear? ResultEquipmentGear2 = await TestService.Context.EquipmentGear.FirstOrDefaultAsync(x => x.TemplateItemGear == generator.Item[1].TemplateItemGear && x.Unit == generator.Unit.Current);
            Assert.IsNotNull(ResultEquipmentGear2);
        }


        [TestMethod]
        public async Task EquipItem_MoreSlotsThanRequired_Success()
        {
            Guild entityGuild = TestService.GetNewGuild();
            Generator generator = new();

            generator.Item.GenerateGear(entityGuild, new() { });
            generator.TemplateSlot.Generate(2);
            generator.SlotGear.Generate(generator.TemplateSlot[0], 2);
            generator.SlotGear.Generate(generator.TemplateSlot[1]);
            generator.Unit.Generate(entityGuild, generator.SlotGear.Generated);
            generator.TemplateItemGearSlot.Generate(generator.Item.Current.TemplateItemGear, generator.TemplateSlot.Generated);

            TestService.Context.SaveChanges();

            await TestService.RPGRepository.EquipGear(entityGuild, generator.Unit.Current.ID, generator.Item.Current.ID, generator.SlotGear.GeneratedID);

            Assert.AreEqual(0, TestService.Context.Item.FirstOrDefault(x => x.ID == generator.Item.Current.ID)?.Count);

            EquipmentGear? ResultEquipmentGear = await TestService.Context.EquipmentGear.FirstOrDefaultAsync(x => x.TemplateItemGear.ID == generator.Item.Current.TemplateItem.ID && x.Unit!.ID == generator.Unit.Current.ID);
            Assert.IsNotNull(ResultEquipmentGear);
        }

        private class Generator
        {
            public ItemGenerator Item { get; set; } = new();
            public UnitGenerator Unit { get; set; } = new();
            public SlotGearGenerator SlotGear { get; set; } = new();
            public TemplateSlotGenerator TemplateSlot { get; set; } = new();
            public TemplateItemGearUseTemplateSlotGenerator TemplateItemGearSlot { get; set; } = new();
        }
    }
}
