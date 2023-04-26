using api.noxy.io.Exceptions;
using api.noxy.io.Models.RPG;
using Database.Models.RPG.Junction;
using Database.Models.RPG;
using TestProject;
using Microsoft.EntityFrameworkCore;

namespace Test.RPGRepository
{
    [TestClass]
    public class EquipItem
    {

        [TestMethod]
        public async Task EquipItem_ArgumentEmpty_Failure()
        {
            Guild entityGuild = TestService.GetNewGuild();
            try
            {
                await TestService.RPGRepository.EquipGear(entityGuild, Guid.Empty, Guid.Empty, new() { Guid.Empty });
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
        public async Task EquipItem_ItemCountZero_Failure()
        {
            Guild entityGuild = TestService.GetNewGuild();
            TemplateItemMaterial TemplateItemGear = TestService.Context.TemplateItemMaterial.Add(new() { Name = "Template Item Gear", Description = string.Empty }).Entity;
            Item Item = TestService.Context.Item.Add(new() { Guild = entityGuild, TemplateItem = TemplateItemGear, Count = 0 }).Entity;

            TestService.Context.SaveChanges();

            try
            {
                await TestService.RPGRepository.EquipGear(entityGuild, Guid.Empty, Item.ID, new() { Guid.Empty });
                Assert.Fail("Call to EquipGear should fail.");
            }
            catch (EntityNotFoundException<Item> ex)
            {
                Type type = ex.Identifier.GetType();
                Assert.AreEqual(entityGuild.ID, (Guid?)type.GetProperty("Guild")?.GetValue(ex.Identifier, null));
                Assert.AreEqual(Item.ID, (Guid?)type.GetProperty("Item")?.GetValue(ex.Identifier, null));
            }
        }

        [TestMethod]
        public async Task EquipItem_ItemWrongTemplate_Failure()
        {
            Guild entityGuild = TestService.GetNewGuild();
            TemplateItemMaterial TemplateItemGear = TestService.Context.TemplateItemMaterial.Add(new() { Name = "Template Item Gear", Description = string.Empty }).Entity;
            Item Item = TestService.Context.Item.Add(new() { Guild = entityGuild, TemplateItem = TemplateItemGear, Count = 1 }).Entity;

            TestService.Context.SaveChanges();

            try
            {
                await TestService.RPGRepository.EquipGear(entityGuild, Guid.Empty, Item.ID, new() { Guid.Empty });
                Assert.Fail("Call to EquipGear should fail.");
            }
            catch (EntityConditionException<Item> ex)
            {
                Assert.AreEqual(Item.ID, (Guid)ex.Identifier);
            }
        }

        [TestMethod]
        public async Task EquipItem_SlotEmpty_Failure()
        {
            Guild entityGuild = TestService.GetNewGuild();
            TemplateSlot TemplateSlot = TestService.Context.TemplateSlot.Add(new() { Name = "Template Slot" }).Entity;
            TemplateItemGear TemplateItemGear = TestService.Context.TemplateItemGear.Add(new() { Name = "Template Item Gear" }).Entity;
            TemplateItemGearWithTemplateSlot TemplateItemGearWithTemplateSlot = TestService.Context.TemplateItemGearWithTemplateSlot.Add(new() { TemplateSlot = TemplateSlot, TemplateItemGear = TemplateItemGear }).Entity;

            Item Item = TestService.Context.Item.Add(new() { Guild = entityGuild, TemplateItem = TemplateItemGear, Count = 1 }).Entity;


            TestService.Context.SaveChanges();

            try
            {
                await TestService.RPGRepository.EquipGear(entityGuild, Guid.Empty, Item.ID, new() { });
                Assert.Fail("Call to EquipGear should fail.");
            }
            catch (EntityNotFoundException<TemplateItemGearWithTemplateSlot> ex)
            {
                Assert.AreEqual(TemplateItemGearWithTemplateSlot.ID, ex.Identifier);
            }
        }

        [TestMethod]
        public async Task EquipItem_SlotWrong_Failure()
        {
            Guild entityGuild = TestService.GetNewGuild();
            TemplateSlot TemplateSlot = TestService.Context.TemplateSlot.Add(new() { Name = "Template Slot" }).Entity;
            TemplateItemGear TemplateItemGear = TestService.Context.TemplateItemGear.Add(new() { Name = "Template Item Gear" }).Entity;
            TemplateItemGearWithTemplateSlot TemplateItemGearWithTemplateSlot = TestService.Context.TemplateItemGearWithTemplateSlot.Add(new() { TemplateSlot = TemplateSlot, TemplateItemGear = TemplateItemGear }).Entity;

            Item Item = TestService.Context.Item.Add(new() { Guild = entityGuild, TemplateItem = TemplateItemGear, Count = 1 }).Entity;

            TestService.Context.SaveChanges();

            List<Guid> SlotGearIDList = new() { Guid.Empty };
            try
            {
                await TestService.RPGRepository.EquipGear(entityGuild, Guid.Empty, Item.ID, SlotGearIDList);
                Assert.Fail("Call to EquipGear should fail.");
            }
            catch (EntityNotFoundException<TemplateItemGearWithTemplateSlot> ex)
            {
                Assert.AreEqual(TemplateItemGearWithTemplateSlot.ID, ex.Identifier);
            }
        }

        [TestMethod]
        public async Task EquipItem_UnitEmpty_Failure()
        {
            Guild entityGuild = TestService.GetNewGuild();
            TemplateSlot TemplateSlot = TestService.Context.TemplateSlot.Add(new() { Name = "Template Slot" }).Entity;
            TemplateItemGear TemplateItemGear = TestService.Context.TemplateItemGear.Add(new() { Name = "Template Item Gear" }).Entity;
            TemplateItemGearWithTemplateSlot TemplateItemGearWithTemplateSlot = TestService.Context.TemplateItemGearWithTemplateSlot.Add(new() { TemplateSlot = TemplateSlot, TemplateItemGear = TemplateItemGear }).Entity;

            SlotGear SlotGear = TestService.Context.SlotGear.Add(new() { Name = "Slot Gear", TemplateSlot = TemplateSlot }).Entity;
            Item Item = TestService.Context.Item.Add(new() { Guild = entityGuild, TemplateItem = TemplateItemGear, Count = 1 }).Entity;

            List<Guid> SlotGearIDList = new() { SlotGear.ID };

            TestService.Context.SaveChanges();

            try
            {
                await TestService.RPGRepository.EquipGear(entityGuild, Guid.Empty, Item.ID, SlotGearIDList);
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
        public async Task EquipItem_UnitHasNoSlot_Failure()
        {
            Guild entityGuild = TestService.GetNewGuild();
            TemplateSlot TemplateSlot = TestService.Context.TemplateSlot.Add(new() { Name = "Template Slot Item" }).Entity;
            SlotGear SlotGear = TestService.Context.SlotGear.Add(new() { Name = "Slot Item", TemplateSlot = TemplateSlot }).Entity;

            TemplateUnit TemplateUnit = TestService.Context.TemplateUnit.Add(new() { Name = "Template Unit", SlotGearList = new() { } }).Entity;
            Unit Unit = TestService.Context.Unit.Add(new Unit() { Guild = entityGuild, TemplateUnit = TemplateUnit, Experience = 0 }).Entity;

            TemplateItemGear TemplateItemGear = TestService.Context.TemplateItemGear.Add(new() { Name = "Template Item Gear" }).Entity;
            Item Item = TestService.Context.Item.Add(new() { Guild = entityGuild, TemplateItem = TemplateItemGear, Count = 1 }).Entity;

            TestService.Context.TemplateItemGearWithTemplateSlot.Add(new() { TemplateSlot = TemplateSlot, TemplateItemGear = TemplateItemGear });

            List<Guid> SlotGearIDList = new() { SlotGear.ID };

            TestService.Context.SaveChanges();

            try
            {
                await TestService.RPGRepository.EquipGear(entityGuild, Unit.ID, Item.ID, SlotGearIDList);
            }
            catch (EntityNotFoundException<SlotGear> ex)
            {
                Assert.IsFalse(SlotGearIDList.Except((IEnumerable<Guid>)ex.Identifier).Any());
            }
        }

        [TestMethod]
        public async Task EquipItem_NoTemplateItemGearWithTemplateSlot_Failure()
        {
            Guild entityGuild = TestService.GetNewGuild();
            TemplateSlot TemplateSlot = TestService.Context.TemplateSlot.Add(new() { Name = "Template Slot Item" }).Entity;
            SlotGear SlotGearItem = TestService.Context.SlotGear.Add(new() { Name = "Slot Item", TemplateSlot = TemplateSlot }).Entity;
            SlotGear SlotGearUnit = TestService.Context.SlotGear.Add(new() { Name = "Slot Unit", TemplateSlot = TemplateSlot }).Entity;

            TemplateUnit TemplateUnit = TestService.Context.TemplateUnit.Add(new() { Name = "Template Unit", SlotGearList = new() { SlotGearUnit } }).Entity;
            Unit Unit = TestService.Context.Unit.Add(new Unit() { Guild = entityGuild, TemplateUnit = TemplateUnit, Experience = 0 }).Entity;

            TemplateItemGear TemplateItemGear = TestService.Context.TemplateItemGear.Add(new() { Name = "Template Item Gear" }).Entity;
            Item Item = TestService.Context.Item.Add(new() { Guild = entityGuild, TemplateItem = TemplateItemGear, Count = 1 }).Entity;

            TestService.Context.TemplateItemGearWithTemplateSlot.Add(new() { TemplateSlot = TemplateSlot, TemplateItemGear = TemplateItemGear });

            List<Guid> SlotGearIDList = new() { SlotGearItem.ID };

            TestService.Context.SaveChanges();

            try
            {
                await TestService.RPGRepository.EquipGear(entityGuild, Unit.ID, Item.ID, SlotGearIDList);
            }
            catch (EntityNotFoundException<SlotGear> ex)
            {
                Assert.IsFalse(SlotGearIDList.Except((IEnumerable<Guid>)ex.Identifier).Any());
            }
        }

        [TestMethod]
        public async Task EquipItem_SameTemplateDifferentSlot_Failure()
        {
            Guild entityGuild = TestService.GetNewGuild();
            TemplateSlot TemplateSlot = TestService.Context.TemplateSlot.Add(new() { Name = "Template Slot Item" }).Entity;
            SlotGear SlotGearItem = TestService.Context.SlotGear.Add(new() { Name = "Slot Item", TemplateSlot = TemplateSlot }).Entity;
            SlotGear SlotGearUnit = TestService.Context.SlotGear.Add(new() { Name = "Slot Unit", TemplateSlot = TemplateSlot }).Entity;

            TemplateUnit TemplateUnit = TestService.Context.TemplateUnit.Add(new() { Name = "Template Unit", SlotGearList = new() { SlotGearUnit } }).Entity;
            Unit Unit = TestService.Context.Unit.Add(new Unit() { Guild = entityGuild, TemplateUnit = TemplateUnit, Experience = 0 }).Entity;

            TemplateItemGear TemplateItemGear = TestService.Context.TemplateItemGear.Add(new() { Name = "Template Item Gear" }).Entity;
            Item Item = TestService.Context.Item.Add(new() { Guild = entityGuild, TemplateItem = TemplateItemGear, Count = 1 }).Entity;

            TestService.Context.TemplateItemGearWithTemplateSlot.Add(new() { TemplateSlot = TemplateSlot, TemplateItemGear = TemplateItemGear });

            List<Guid> SlotGearIDList = new() { SlotGearItem.ID };

            TestService.Context.SaveChanges();

            try
            {
                await TestService.RPGRepository.EquipGear(entityGuild, Unit.ID, Item.ID, SlotGearIDList);
            }
            catch (EntityNotFoundException<SlotGear> ex)
            {
                Assert.IsFalse(SlotGearIDList.Except((IEnumerable<Guid>)ex.Identifier).Any());
            }
        }

        [TestMethod]
        public async Task EquipItem_NotEnoughGearSlotGiven_Failure()
        {
            Guild entityGuild = TestService.GetNewGuild();
            TemplateSlot TemplateSlot = TestService.Context.TemplateSlot.Add(new() { Name = "Template Slot Item" }).Entity;
            SlotGear SlotGearOne = TestService.Context.SlotGear.Add(new() { Name = "Slot #1", TemplateSlot = TemplateSlot }).Entity;
            SlotGear SlotGearTwo = TestService.Context.SlotGear.Add(new() { Name = "Slot #2", TemplateSlot = TemplateSlot }).Entity;

            TemplateUnit TemplateUnit = TestService.Context.TemplateUnit.Add(new() { Name = "Template Unit", SlotGearList = new() { SlotGearOne, SlotGearTwo } }).Entity;
            Unit Unit = TestService.Context.Unit.Add(new Unit() { Guild = entityGuild, TemplateUnit = TemplateUnit, Experience = 0 }).Entity;

            TemplateItemGear TemplateItemGear = TestService.Context.TemplateItemGear.Add(new() { Name = "Template Item Gear" }).Entity;
            Item Item = TestService.Context.Item.Add(new() { Guild = entityGuild, TemplateItem = TemplateItemGear, Count = 1 }).Entity;

            TemplateItemGearWithTemplateSlot TemplateItemGearWithTemplateSlotOne = TestService.Context.TemplateItemGearWithTemplateSlot.Add(new() { TemplateSlot = TemplateSlot, TemplateItemGear = TemplateItemGear }).Entity;
            TemplateItemGearWithTemplateSlot TemplateItemGearWithTemplateSlotTwo = TestService.Context.TemplateItemGearWithTemplateSlot.Add(new() { TemplateSlot = TemplateSlot, TemplateItemGear = TemplateItemGear }).Entity;

            List<Guid> SlotGearIDList = new() { SlotGearOne.ID };

            TestService.Context.SaveChanges();

            try
            {
                await TestService.RPGRepository.EquipGear(entityGuild, Unit.ID, Item.ID, SlotGearIDList);
            }
            catch (EntityNotFoundException<TemplateItemGearWithTemplateSlot> ex)
            {
                Assert.AreNotEqual(TemplateItemGearWithTemplateSlotOne.ID, ex.Identifier);
                Assert.AreEqual(TemplateItemGearWithTemplateSlotTwo.ID, ex.Identifier);
            }
        }

        [TestMethod]
        public async Task EquipItem_Basic_Success()
        {
            Guild entityGuild = TestService.GetNewGuild();
            TemplateSlot TemplateSlot = TestService.Context.TemplateSlot.Add(new() { Name = "Template Slot" }).Entity;
            SlotGear SlotGear = TestService.Context.SlotGear.Add(new() { Name = "Slot Gear", TemplateSlot = TemplateSlot }).Entity;

            TemplateUnit TemplateUnit = TestService.Context.TemplateUnit.Add(new() { Name = "Template Unit", SlotGearList = new() { SlotGear } }).Entity;
            Unit Unit = TestService.Context.Unit.Add(new Unit() { Guild = entityGuild, TemplateUnit = TemplateUnit, Experience = 0 }).Entity;

            TemplateItemGear TemplateItemGear = TestService.Context.TemplateItemGear.Add(new() { Name = "Template Item Gear" }).Entity;
            Item Item = TestService.Context.Item.Add(new() { Guild = entityGuild, TemplateItem = TemplateItemGear, Count = 1 }).Entity;

            TestService.Context.TemplateItemGearWithTemplateSlot.Add(new() { TemplateSlot = TemplateSlot, TemplateItemGear = TemplateItemGear });

            TestService.Context.SaveChanges();

            await TestService.RPGRepository.EquipGear(entityGuild, Unit.ID, Item.ID, new() { SlotGear.ID });

            Item? ResultItem = TestService.Context.Item.FirstOrDefault(x => x.ID == Item.ID);
            Assert.IsNotNull(ResultItem);
            Assert.AreEqual(0, ResultItem?.Count);

            EquipmentGear? ResultEquipmentGear = TestService.Context.EquipmentGear.FirstOrDefault(x => x.TemplateItemGear.ID == TemplateItemGear.ID && x.Unit!.ID == Unit.ID);
            Assert.IsNotNull(ResultEquipmentGear);

            int ResultTemplateItemGearTemplateSlot = TestService.Context.TemplateItemGearWithTemplateSlot.Count(x => x.TemplateItemGear.ID == TemplateItemGear.ID && x.TemplateSlot.ID == TemplateSlot.ID);
            Assert.AreEqual(1, ResultTemplateItemGearTemplateSlot);
        }

        [TestMethod]
        public async Task EquipItem_TwoOfTheSameTemplateSlot_Success()
        {
            Guild entityGuild = TestService.GetNewGuild();
            TemplateSlot TemplateSlot = TestService.Context.TemplateSlot.Add(new() { Name = "Template Slot" }).Entity;
            SlotGear SlotGear1 = TestService.Context.SlotGear.Add(new() { Name = "Slot Gear 1", TemplateSlot = TemplateSlot }).Entity;
            SlotGear SlotGear2 = TestService.Context.SlotGear.Add(new() { Name = "Slot Gear 2", TemplateSlot = TemplateSlot }).Entity;

            TemplateUnit TemplateUnit = TestService.Context.TemplateUnit.Add(new() { Name = "Template Unit", SlotGearList = new() { SlotGear1, SlotGear2 } }).Entity;
            Unit Unit = TestService.Context.Unit.Add(new Unit() { Guild = entityGuild, TemplateUnit = TemplateUnit, Experience = 0 }).Entity;

            TemplateItemGear TemplateItemGear = TestService.Context.TemplateItemGear.Add(new() { Name = "Template Item Gear" }).Entity;
            Item Item = TestService.Context.Item.Add(new() { Guild = entityGuild, TemplateItem = TemplateItemGear, Count = 1 }).Entity;

            TestService.Context.TemplateItemGearWithTemplateSlot.Add(new() { TemplateSlot = TemplateSlot, TemplateItemGear = TemplateItemGear });
            TestService.Context.TemplateItemGearWithTemplateSlot.Add(new() { TemplateSlot = TemplateSlot, TemplateItemGear = TemplateItemGear });

            TestService.Context.SaveChanges();

            await TestService.RPGRepository.EquipGear(entityGuild, Unit.ID, Item.ID, new() { SlotGear1.ID, SlotGear2.ID });

            Item? ResultItem = TestService.Context.Item.FirstOrDefault(x => x.ID == Item.ID);
            Assert.IsNotNull(ResultItem);
            Assert.AreEqual(0, ResultItem?.Count);

            EquipmentGear? ResultEquipmentGear = TestService.Context.EquipmentGear.FirstOrDefault(x => x.TemplateItemGear.ID == TemplateItemGear.ID && x.Unit!.ID == Unit.ID);
            Assert.IsNotNull(ResultEquipmentGear);
        }

        [TestMethod]
        public async Task EquipItem_EquipSameItemTwice_Success()
        {
            Guild entityGuild = TestService.GetNewGuild();
            TemplateSlot TemplateSlot = TestService.Context.TemplateSlot.Add(new() { Name = "Template Slot" }).Entity;
            SlotGear SlotGear = TestService.Context.SlotGear.Add(new() { Name = "Slot Gear 1", TemplateSlot = TemplateSlot }).Entity;

            TemplateUnit TemplateUnit = TestService.Context.TemplateUnit.Add(new() { Name = "Template Unit", SlotGearList = new() { SlotGear } }).Entity;
            Unit Unit = TestService.Context.Unit.Add(new Unit() { Guild = entityGuild, TemplateUnit = TemplateUnit, Experience = 0 }).Entity;

            TemplateItemGear TemplateItemGear = TestService.Context.TemplateItemGear.Add(new() { Name = "Template Item Gear" }).Entity;
            Item Item = TestService.Context.Item.Add(new() { Guild = entityGuild, TemplateItem = TemplateItemGear, Count = 2 }).Entity;

            TestService.Context.TemplateItemGearWithTemplateSlot.Add(new() { TemplateSlot = TemplateSlot, TemplateItemGear = TemplateItemGear });

            TestService.Context.SaveChanges();

            await TestService.RPGRepository.EquipGear(entityGuild, Unit.ID, Item.ID, new() { SlotGear.ID });
            await TestService.RPGRepository.EquipGear(entityGuild, Unit.ID, Item.ID, new() { SlotGear.ID });

            Item? ResultItem = TestService.Context.Item.FirstOrDefault(x => x.ID == Item.ID);
            Assert.IsNotNull(ResultItem);
            Assert.AreEqual(0, ResultItem?.Count);

            int ResultEquipmentGear = TestService.Context.EquipmentGear.Count(x => x.TemplateItemGear.ID == TemplateItemGear.ID && x.Unit!.ID == Unit.ID);
            Assert.AreEqual(2, ResultEquipmentGear);
        }


        [TestMethod]
        public async Task EquipItem_TwoOfThreeSlotOverlap_Success()
        {
            Guild entityGuild = TestService.GetNewGuild();
            TemplateSlot TemplateSlot1 = TestService.Context.TemplateSlot.Add(new() { Name = "Template Slot1" }).Entity;
            TemplateSlot TemplateSlot2 = TestService.Context.TemplateSlot.Add(new() { Name = "Template Slot2" }).Entity;
            TemplateSlot TemplateSlot3 = TestService.Context.TemplateSlot.Add(new() { Name = "Template Slot3" }).Entity;
            SlotGear SlotGear1 = TestService.Context.SlotGear.Add(new() { Name = "Slot Gear 1", TemplateSlot = TemplateSlot1 }).Entity;
            SlotGear SlotGear2 = TestService.Context.SlotGear.Add(new() { Name = "Slot Gear 2", TemplateSlot = TemplateSlot2 }).Entity;
            SlotGear SlotGear3 = TestService.Context.SlotGear.Add(new() { Name = "Slot Gear 3", TemplateSlot = TemplateSlot3 }).Entity;

            TemplateUnit TemplateUnit = TestService.Context.TemplateUnit.Add(new() { Name = "Template Unit", SlotGearList = new() { SlotGear1, SlotGear2 } }).Entity;
            Unit Unit = TestService.Context.Unit.Add(new Unit() { Guild = entityGuild, TemplateUnit = TemplateUnit, Experience = 0 }).Entity;

            TemplateItemGear TemplateItemGear1 = TestService.Context.TemplateItemGear.Add(new() { Name = "Template Item Gear 1" }).Entity;
            TemplateItemGear TemplateItemGear2 = TestService.Context.TemplateItemGear.Add(new() { Name = "Template Item Gear 2" }).Entity;
            
            Item Item1 = TestService.Context.Item.Add(new() { Guild = entityGuild, TemplateItem = TemplateItemGear1, Count = 1 }).Entity;
            Item Item2 = TestService.Context.Item.Add(new() { Guild = entityGuild, TemplateItem = TemplateItemGear2, Count = 1 }).Entity;

            TestService.Context.TemplateItemGearWithTemplateSlot.Add(new() { TemplateSlot = TemplateSlot1, TemplateItemGear = TemplateItemGear1 });
            TestService.Context.TemplateItemGearWithTemplateSlot.Add(new() { TemplateSlot = TemplateSlot2, TemplateItemGear = TemplateItemGear1 });
            TestService.Context.TemplateItemGearWithTemplateSlot.Add(new() { TemplateSlot = TemplateSlot2, TemplateItemGear = TemplateItemGear2 });
            TestService.Context.TemplateItemGearWithTemplateSlot.Add(new() { TemplateSlot = TemplateSlot3, TemplateItemGear = TemplateItemGear2 });

            TestService.Context.SaveChanges();

            await TestService.RPGRepository.EquipGear(entityGuild, Unit.ID, Item1.ID, new() { SlotGear1.ID, SlotGear2.ID });
            await TestService.RPGRepository.EquipGear(entityGuild, Unit.ID, Item2.ID, new() { SlotGear2.ID, SlotGear3.ID });

            Assert.AreEqual(0, await TestService.Context.Item.CountAsync(x => x.ID == Item1.ID));
            Assert.AreEqual(0, await TestService.Context.Item.CountAsync(x => x.ID == Item2.ID));

            EquipmentGear? ResultEquipmentGear1 = await TestService.Context.EquipmentGear.FirstOrDefaultAsync(x => x.TemplateItemGear.ID == TemplateItemGear1.ID && x.Unit!.ID == Unit.ID);
            Assert.IsNull(ResultEquipmentGear1);

            EquipmentGear? ResultEquipmentGear2 = await TestService.Context.EquipmentGear.FirstOrDefaultAsync(x => x.TemplateItemGear.ID == TemplateItemGear2.ID && x.Unit!.ID == Unit.ID);
            Assert.IsNotNull(ResultEquipmentGear2);
        }
    }
}
