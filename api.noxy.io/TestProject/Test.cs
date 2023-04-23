using api.noxy.io.Context;
using api.noxy.io.Exceptions;
using api.noxy.io.Interface;
using api.noxy.io.Models.RPG;
using api.noxy.io.Utility;
using Database.Models.RPG;
using Database.Models.RPG.Junction;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace TestProject
{
    [TestClass]
    public class Test
    {
        private readonly RPGContext Context;
        private readonly IRPGRepository RPGRepository;
        private readonly IAuthRepository AuthRepository;

        private readonly User User;
        private readonly Guild Guild;

        public Test()
        {
            IConfigurationRoot config = new ConfigurationBuilder().AddJsonFile("appsettings.json", optional: true, reloadOnChange: true).Build();
            ApplicationConfiguration appconfig = new ApplicationConfiguration(config);

            ServiceCollection services = new ServiceCollection();
            services.AddScoped<IConfiguration>(_ => config);
            services.AddTransient<IApplicationConfiguration, ApplicationConfiguration>();
            services.AddDbContext<RPGContext>(options => options.UseMySQL(appconfig.Database.ConnectionString));
            services.AddTransient<IRPGRepository, RPGRepository>();
            services.AddTransient<IAuthRepository, AuthRepository>();

            ServiceProvider serviceProvider = services.BuildServiceProvider();
            Context = serviceProvider.GetRequiredService<RPGContext>();
            RPGRepository = serviceProvider.GetRequiredService<IRPGRepository>();
            AuthRepository = serviceProvider.GetRequiredService<IAuthRepository>();

            RPGContext dbContext = serviceProvider.GetRequiredService<RPGContext>();
            Context.Database.EnsureDeleted();
            Context.Database.EnsureCreated();

            byte[] salt = User.GenerateSalt();
            byte[] hash = User.GenerateHash("Test1234", salt);

            User = Context.User.Add(new() { Email = "test@example.com", Salt = salt, Hash = hash }).Entity;
            Guild = Context.Guild.Add(new() { Name = "Test", User = User }).Entity;

            Context.SaveChanges();
        }

        [TestMethod]
        public async Task LoadGuildSuccessTest()
        {
            Guild? entityGuild = await RPGRepository.LoadGuild(User);
            Assert.IsNotNull(entityGuild);
            Assert.AreEqual("Test", entityGuild.Name);
            Assert.AreEqual("test@example.com", entityGuild.User.Email);
        }

        [TestMethod]
        public async Task CraftItemNoRecipeUnlockedFailure()
        {
            TemplateRecipe TemplateRecipe = Context.TemplateRecipe.Add(new() { Name = "Recipe: Equipment" }).Entity;
            Context.SaveChanges();

            try
            {
                await RPGRepository.CraftItem(Guild, TemplateRecipe.ID, 1, new List<Guid>() { });
                Assert.Fail("Guild should not have access to this recipe");
            }
            catch (EntityNotFoundException<UnlockableRecipe> ex)
            {
                Type type = ex.Identifier.GetType();
                Assert.AreEqual(Guild.ID, (Guid?)type.GetProperty("Guild")?.GetValue(ex.Identifier, null));
                Assert.AreEqual(TemplateRecipe.ID, (Guid?)type.GetProperty("Recipe")?.GetValue(ex.Identifier, null));
            }
        }


        [TestMethod]
        public async Task CraftItemWrongMaterialFailure()
        {
            TemplateItemMaterial TemplateItemMaterialSuccess = Context.TemplateItemMaterial.Add(new() { Name = "Success Material", Description = string.Empty }).Entity;
            TemplateItemMaterial TemplateItemMaterialFailure = Context.TemplateItemMaterial.Add(new() { Name = "Failure Material", Description = string.Empty }).Entity;

            TemplateRecipe TemplateRecipe = Context.TemplateRecipe.Add(new() { Name = "Recipe: Equipment" }).Entity;
            Context.UnlockableRecipe.Add(new() { Guild = Guild, TemplateRecipe = TemplateRecipe });
            Context.VolumeItemRecipe.Add(new() { TemplateRecipe = TemplateRecipe, Component = true, TemplateItem = TemplateItemMaterialSuccess, Count = 5 });

            Item Item = Context.Item.Add(new Item() { Guild = Guild, TemplateItem = TemplateItemMaterialFailure, Count = 5 }).Entity;

            Context.SaveChanges();

            try
            {
                await RPGRepository.CraftItem(Guild, TemplateRecipe.ID, 1, new List<Guid>() { Item.ID });
                Assert.Fail("Call to CraftItem should fail.");
            }
            catch (EntityNotFoundException<Item> ex)
            {
                Type type = ex.Identifier.GetType();
                Assert.AreEqual(Guild.ID, (Guid?)type.GetProperty("Guild")?.GetValue(ex.Identifier, null));
                Assert.AreEqual(TemplateItemMaterialSuccess.ID, (Guid?)type.GetProperty("TemplateItem")?.GetValue(ex.Identifier, null));
            }
        }

        [TestMethod]
        public async Task CraftItemInsufficientMaterialFailure()
        {
            TemplateItemMaterial TemplateItemMaterial = Context.TemplateItemMaterial.Add(new() { Name = "Material", Description = string.Empty }).Entity;

            TemplateRecipe TemplateRecipe = Context.TemplateRecipe.Add(new() { Name = "Recipe: Equipment" }).Entity;
            Context.UnlockableRecipe.Add(new() { Guild = Guild, TemplateRecipe = TemplateRecipe });
            Context.VolumeItemRecipe.Add(new() { TemplateRecipe = TemplateRecipe, Component = true, TemplateItem = TemplateItemMaterial, Count = 5 });

            Item Item = Context.Item.Add(new Item() { Guild = Guild, TemplateItem = TemplateItemMaterial, Count = 2 }).Entity;

            Context.SaveChanges();

            try
            {
                await RPGRepository.CraftItem(Guild, TemplateRecipe.ID, 1, new List<Guid>() { Item.ID });
                Assert.Fail("Call to CraftItem should fail.");
            }
            catch (EntityConditionException<Item> ex)
            {
                Assert.AreEqual(Item.ID, (Guid)ex.Identifier);
                return;
            }
        }

        [TestMethod]
        public async Task CraftItemSuccess()
        {
            TemplateItemMaterial TemplateItemMaterial = Context.TemplateItemMaterial.Add(new() { Name = "Material", Description = string.Empty }).Entity;
            TemplateItemGear TemplateItemGear = Context.TemplateItemGear.Add(new() { Name = "Gear" }).Entity;

            TemplateRecipe TemplateRecipe = Context.TemplateRecipe.Add(new() { Name = "Recipe: Equipment" }).Entity;
            Context.UnlockableRecipe.Add(new() { Guild = Guild, TemplateRecipe = TemplateRecipe });
            Context.VolumeItemRecipe.Add(new() { TemplateRecipe = TemplateRecipe, Component = true, TemplateItem = TemplateItemMaterial, Count = 5 });
            Context.VolumeItemRecipe.Add(new() { TemplateRecipe = TemplateRecipe, Component = false, TemplateItem = TemplateItemGear, Count = 1 });

            Item Item = Context.Item.Add(new Item() { Guild = Guild, TemplateItem = TemplateItemMaterial, Count = 5 }).Entity;

            Context.SaveChanges();

            await RPGRepository.CraftItem(Guild, TemplateRecipe.ID, 1, new List<Guid>() { Item.ID });

            Item? ItemInput = await Context.Item.FirstOrDefaultAsync(x => x.TemplateItem.ID == TemplateItemMaterial.ID && x.Guild.ID == Guild.ID);
            Assert.IsNotNull(ItemInput);
            Assert.AreEqual(0, ItemInput.Count);

            Item? ItemOutput = await Context.Item.FirstOrDefaultAsync(x => x.TemplateItem.ID == TemplateItemGear.ID && x.Guild.ID == Guild.ID);
            Assert.IsNotNull(ItemOutput);
            Assert.AreEqual(1, ItemOutput.Count);
        }

        [TestMethod]
        public async Task CraftItemFromMultipleMaterialSuccess()
        {
            TemplateItemMaterial TemplateItemMaterial1 = Context.TemplateItemMaterial.Add(new() { Name = "Material 1", Description = string.Empty }).Entity;
            TemplateItemMaterial TemplateItemMaterial2 = Context.TemplateItemMaterial.Add(new() { Name = "Material 2", Description = string.Empty }).Entity;
            TemplateItemGear TemplateItemGear = Context.TemplateItemGear.Add(new() { Name = "Gear" }).Entity;

            TemplateRecipe TemplateRecipe = Context.TemplateRecipe.Add(new() { Name = "Recipe: Equipment" }).Entity;
            Context.UnlockableRecipe.Add(new() { Guild = Guild, TemplateRecipe = TemplateRecipe });
            Context.VolumeItemRecipe.Add(new() { TemplateRecipe = TemplateRecipe, Component = true, TemplateItem = TemplateItemMaterial1, Count = 5 });
            Context.VolumeItemRecipe.Add(new() { TemplateRecipe = TemplateRecipe, Component = true, TemplateItem = TemplateItemMaterial2, Count = 3 });
            Context.VolumeItemRecipe.Add(new() { TemplateRecipe = TemplateRecipe, Component = false, TemplateItem = TemplateItemGear, Count = 1 });

            Item ItemMaterial1 = Context.Item.Add(new Item() { Guild = Guild, TemplateItem = TemplateItemMaterial1, Count = 5 }).Entity;
            Item ItemMaterial2 = Context.Item.Add(new Item() { Guild = Guild, TemplateItem = TemplateItemMaterial2, Count = 3 }).Entity;

            Context.SaveChanges();

            await RPGRepository.CraftItem(Guild, TemplateRecipe.ID, 1, new List<Guid>() { ItemMaterial1.ID, ItemMaterial2.ID });
        }

        [TestMethod]
        public async Task EquipItem_ArgumentEmpty_Failure()
        {
            try
            {
                await RPGRepository.EquipGear(Guild, Guid.Empty, Guid.Empty, new() { Guid.Empty });
                Assert.Fail("Call to EquipGear should fail.");
            }
            catch (EntityNotFoundException<Item> ex)
            {
                Type type = ex.Identifier.GetType();
                Assert.AreEqual(Guild.ID, (Guid?)type.GetProperty("Guild")?.GetValue(ex.Identifier, null));
                Assert.AreEqual(Guid.Empty, (Guid?)type.GetProperty("Item")?.GetValue(ex.Identifier, null));
            }
        }

        [TestMethod]
        public async Task EquipItem_ItemCountZero_Failure()
        {
            TemplateItemMaterial TemplateItemGear = Context.TemplateItemMaterial.Add(new() { Name = "Template Item Gear", Description = string.Empty }).Entity;
            Item Item = Context.Item.Add(new() { Guild = Guild, TemplateItem = TemplateItemGear, Count = 0 }).Entity;

            Context.SaveChanges();

            try
            {
                await RPGRepository.EquipGear(Guild, Guid.Empty, Item.ID, new() { Guid.Empty });
                Assert.Fail("Call to EquipGear should fail.");
            }
            catch (EntityNotFoundException<Item> ex)
            {
                Type type = ex.Identifier.GetType();
                Assert.AreEqual(Guild.ID, (Guid?)type.GetProperty("Guild")?.GetValue(ex.Identifier, null));
                Assert.AreEqual(Item.ID, (Guid?)type.GetProperty("Item")?.GetValue(ex.Identifier, null));
            }
        }

        [TestMethod]
        public async Task EquipItem_ItemWrongTemplate_Failure()
        {
            TemplateItemMaterial TemplateItemGear = Context.TemplateItemMaterial.Add(new() { Name = "Template Item Gear", Description = string.Empty }).Entity;
            Item Item = Context.Item.Add(new() { Guild = Guild, TemplateItem = TemplateItemGear, Count = 1 }).Entity;

            Context.SaveChanges();

            try
            {
                await RPGRepository.EquipGear(Guild, Guid.Empty, Item.ID, new() { Guid.Empty });
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
            TemplateSlot TemplateSlot = Context.TemplateSlot.Add(new() { Name = "Template Slot" }).Entity;
            TemplateItemGear TemplateItemGear = Context.TemplateItemGear.Add(new() { Name = "Template Item Gear" }).Entity;
            TemplateItemGearWithTemplateSlot TemplateItemGearWithTemplateSlot = Context.TemplateItemGearWithTemplateSlot.Add(new() { TemplateSlot = TemplateSlot, TemplateItemGear = TemplateItemGear }).Entity;

            Item Item = Context.Item.Add(new() { Guild = Guild, TemplateItem = TemplateItemGear, Count = 1 }).Entity;


            Context.SaveChanges();

            try
            {
                await RPGRepository.EquipGear(Guild, Guid.Empty, Item.ID, new() { });
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
            TemplateSlot TemplateSlot = Context.TemplateSlot.Add(new() { Name = "Template Slot" }).Entity;
            TemplateItemGear TemplateItemGear = Context.TemplateItemGear.Add(new() { Name = "Template Item Gear" }).Entity;
            TemplateItemGearWithTemplateSlot TemplateItemGearWithTemplateSlot = Context.TemplateItemGearWithTemplateSlot.Add(new() { TemplateSlot = TemplateSlot, TemplateItemGear = TemplateItemGear }).Entity;

            Item Item = Context.Item.Add(new() { Guild = Guild, TemplateItem = TemplateItemGear, Count = 1 }).Entity;

            Context.SaveChanges();

            List<Guid> SlotGearIDList = new() { Guid.Empty };
            try
            {
                await RPGRepository.EquipGear(Guild, Guid.Empty, Item.ID, SlotGearIDList);
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
            TemplateSlot TemplateSlot = Context.TemplateSlot.Add(new() { Name = "Template Slot" }).Entity;
            TemplateItemGear TemplateItemGear = Context.TemplateItemGear.Add(new() { Name = "Template Item Gear" }).Entity;
            TemplateItemGearWithTemplateSlot TemplateItemGearWithTemplateSlot = Context.TemplateItemGearWithTemplateSlot.Add(new() { TemplateSlot = TemplateSlot, TemplateItemGear = TemplateItemGear }).Entity;

            SlotGear SlotGear = Context.SlotGear.Add(new() { Name = "Slot Gear", TemplateSlot = TemplateSlot }).Entity;
            Item Item = Context.Item.Add(new() { Guild = Guild, TemplateItem = TemplateItemGear, Count = 1 }).Entity;

            List<Guid> SlotGearIDList = new() { SlotGear.ID };

            Context.SaveChanges();

            try
            {
                await RPGRepository.EquipGear(Guild, Guid.Empty, Item.ID, SlotGearIDList);
                Assert.Fail("Call to EquipGear should fail.");
            }
            catch (EntityNotFoundException<Unit> ex)
            {
                Type type = ex.Identifier.GetType();
                Assert.AreEqual(Guild.ID, (Guid?)type.GetProperty("Guild")?.GetValue(ex.Identifier, null));
                Assert.AreEqual(Guid.Empty, (Guid?)type.GetProperty("Unit")?.GetValue(ex.Identifier, null));
            }
        }

        [TestMethod]
        public async Task EquipItem_UnitHasNoSlot_Failure()
        {
            TemplateSlot TemplateSlot = Context.TemplateSlot.Add(new() { Name = "Template Slot Item" }).Entity;
            SlotGear SlotGear = Context.SlotGear.Add(new() { Name = "Slot Item", TemplateSlot = TemplateSlot }).Entity;

            TemplateUnit TemplateUnit = Context.TemplateUnit.Add(new() { Name = "Template Unit", SlotGearList = new() { } }).Entity;
            Unit Unit = Context.Unit.Add(new Unit() { Guild = Guild, TemplateUnit = TemplateUnit, Experience = 0 }).Entity;

            TemplateItemGear TemplateItemGear = Context.TemplateItemGear.Add(new() { Name = "Template Item Gear" }).Entity;
            Item Item = Context.Item.Add(new() { Guild = Guild, TemplateItem = TemplateItemGear, Count = 1 }).Entity;

            Context.TemplateItemGearWithTemplateSlot.Add(new() { TemplateSlot = TemplateSlot, TemplateItemGear = TemplateItemGear });

            List<Guid> SlotGearIDList = new() { SlotGear.ID };

            Context.SaveChanges();

            try
            {
                await RPGRepository.EquipGear(Guild, Unit.ID, Item.ID, SlotGearIDList);
            }
            catch (EntityNotFoundException<SlotGear> ex)
            {
                Assert.IsFalse(SlotGearIDList.Except((IEnumerable<Guid>)ex.Identifier).Any());
            }
        }


        [TestMethod]
        /// <summary>
        /// A Player tries to equip a Unit with a Ring in the Left Ring Slot, but the equipping Unit only has a Right Ring Slot, therefore the test fails.
        /// </summary>
        public async Task EquipItem_NoTemplateItemGearWithTemplateSlot_Failure()
        {
            TemplateSlot TemplateSlot = Context.TemplateSlot.Add(new() { Name = "Template Slot Item" }).Entity;
            SlotGear SlotGearItem = Context.SlotGear.Add(new() { Name = "Slot Item", TemplateSlot = TemplateSlot }).Entity;
            SlotGear SlotGearUnit = Context.SlotGear.Add(new() { Name = "Slot Unit", TemplateSlot = TemplateSlot }).Entity;

            TemplateUnit TemplateUnit = Context.TemplateUnit.Add(new() { Name = "Template Unit", SlotGearList = new() { SlotGearUnit } }).Entity;
            Unit Unit = Context.Unit.Add(new Unit() { Guild = Guild, TemplateUnit = TemplateUnit, Experience = 0 }).Entity;

            TemplateItemGear TemplateItemGear = Context.TemplateItemGear.Add(new() { Name = "Template Item Gear" }).Entity;
            Item Item = Context.Item.Add(new() { Guild = Guild, TemplateItem = TemplateItemGear, Count = 1 }).Entity;

            Context.TemplateItemGearWithTemplateSlot.Add(new() { TemplateSlot = TemplateSlot, TemplateItemGear = TemplateItemGear });

            List<Guid> SlotGearIDList = new() { SlotGearItem.ID };

            Context.SaveChanges();

            try
            {
                await RPGRepository.EquipGear(Guild, Unit.ID, Item.ID, SlotGearIDList);
            }
            catch (EntityNotFoundException<SlotGear> ex)
            {
                Assert.IsFalse(SlotGearIDList.Except((IEnumerable<Guid>)ex.Identifier).Any());
            }
        }

        [TestMethod]
        /// <summary>
        /// A Player tries to equip a Unit with a Ring in the Left Ring Slot, but the equipping Unit only has a Right Ring Slot, therefore the test fails.
        /// </summary>
        public async Task EquipItem_SameTemplateDifferentSlot_Failure()
        {
            TemplateSlot TemplateSlot = Context.TemplateSlot.Add(new() { Name = "Template Slot Item" }).Entity;
            SlotGear SlotGearItem = Context.SlotGear.Add(new() { Name = "Slot Item", TemplateSlot = TemplateSlot }).Entity;
            SlotGear SlotGearUnit = Context.SlotGear.Add(new() { Name = "Slot Unit", TemplateSlot = TemplateSlot }).Entity;

            TemplateUnit TemplateUnit = Context.TemplateUnit.Add(new() { Name = "Template Unit", SlotGearList = new() { SlotGearUnit } }).Entity;
            Unit Unit = Context.Unit.Add(new Unit() { Guild = Guild, TemplateUnit = TemplateUnit, Experience = 0 }).Entity;

            TemplateItemGear TemplateItemGear = Context.TemplateItemGear.Add(new() { Name = "Template Item Gear" }).Entity;
            Item Item = Context.Item.Add(new() { Guild = Guild, TemplateItem = TemplateItemGear, Count = 1 }).Entity;

            Context.TemplateItemGearWithTemplateSlot.Add(new() { TemplateSlot = TemplateSlot, TemplateItemGear = TemplateItemGear });

            List<Guid> SlotGearIDList = new() { SlotGearItem.ID };

            Context.SaveChanges();

            try
            {
                await RPGRepository.EquipGear(Guild, Unit.ID, Item.ID, SlotGearIDList);
            }
            catch (EntityNotFoundException<SlotGear> ex)
            {
                Assert.IsFalse(SlotGearIDList.Except((IEnumerable<Guid>)ex.Identifier).Any());
            }
        }


        [TestMethod]
        public async Task EquipItem_NotEnoughGearSlotGiven_Failure()
        {
            TemplateSlot TemplateSlot = Context.TemplateSlot.Add(new() { Name = "Template Slot Item" }).Entity;
            SlotGear SlotGearOne = Context.SlotGear.Add(new() { Name = "Slot #1", TemplateSlot = TemplateSlot }).Entity;
            SlotGear SlotGearTwo = Context.SlotGear.Add(new() { Name = "Slot #2", TemplateSlot = TemplateSlot }).Entity;

            TemplateUnit TemplateUnit = Context.TemplateUnit.Add(new() { Name = "Template Unit", SlotGearList = new() { SlotGearOne, SlotGearTwo } }).Entity;
            Unit Unit = Context.Unit.Add(new Unit() { Guild = Guild, TemplateUnit = TemplateUnit, Experience = 0 }).Entity;

            TemplateItemGear TemplateItemGear = Context.TemplateItemGear.Add(new() { Name = "Template Item Gear" }).Entity;
            Item Item = Context.Item.Add(new() { Guild = Guild, TemplateItem = TemplateItemGear, Count = 1 }).Entity;

            TemplateItemGearWithTemplateSlot TemplateItemGearWithTemplateSlotOne = Context.TemplateItemGearWithTemplateSlot.Add(new() { TemplateSlot = TemplateSlot, TemplateItemGear = TemplateItemGear }).Entity;
            TemplateItemGearWithTemplateSlot TemplateItemGearWithTemplateSlotTwo = Context.TemplateItemGearWithTemplateSlot.Add(new() { TemplateSlot = TemplateSlot, TemplateItemGear = TemplateItemGear }).Entity;

            List<Guid> SlotGearIDList = new() { SlotGearOne.ID };

            Context.SaveChanges();

            try
            {
                await RPGRepository.EquipGear(Guild, Unit.ID, Item.ID, SlotGearIDList);
            }
            catch (EntityNotFoundException<TemplateItemGearWithTemplateSlot> ex)
            {
                Assert.AreNotEqual(TemplateItemGearWithTemplateSlotOne.ID, ex.Identifier);
                Assert.AreEqual(TemplateItemGearWithTemplateSlotTwo.ID, ex.Identifier);
            }
        }

        [TestMethod]
        public async Task SomeTest()
        {
            TemplateSlot TemplateSlot = Context.TemplateSlot.Add(new() { Name = "Template Slot Item" }).Entity;
            SlotGear SlotGear1 = Context.SlotGear.Add(new() { Name = "Slot #1", TemplateSlot = TemplateSlot }).Entity;
            SlotGear SlotGear2 = Context.SlotGear.Add(new() { Name = "Slot #2", TemplateSlot = TemplateSlot }).Entity;

            TemplateItemGear TemplateItemGear = Context.TemplateItemGear.Add(new() { Name = "Template Item Gear" }).Entity;

            Context.TemplateItemGearWithTemplateSlot.Add(new() { TemplateSlot = TemplateSlot, TemplateItemGear = TemplateItemGear });
            Context.TemplateItemGearWithTemplateSlot.Add(new() { TemplateSlot = TemplateSlot, TemplateItemGear = TemplateItemGear });

            Context.SaveChanges();
        }


        [TestMethod]
        public async Task EquipItem()
        {
            TemplateSlot TemplateSlot = Context.TemplateSlot.Add(new() { Name = "Template Slot" }).Entity;
            SlotGear SlotGear = Context.SlotGear.Add(new() { Name = "Slot Gear", TemplateSlot = TemplateSlot }).Entity;

            TemplateUnit TemplateUnit = Context.TemplateUnit.Add(new() { Name = "Template Unit", SlotGearList = new() { SlotGear } }).Entity;
            Unit Unit = Context.Unit.Add(new Unit() { Guild = Guild, TemplateUnit = TemplateUnit, Experience = 0 }).Entity;

            TemplateItemGear TemplateItemGear = Context.TemplateItemGear.Add(new() { Name = "Template Item Gear" }).Entity;
            Item Item = Context.Item.Add(new() { Guild = Guild, TemplateItem = TemplateItemGear, Count = 1 }).Entity;

            Context.TemplateItemGearWithTemplateSlot.Add(new() { TemplateSlot = TemplateSlot, TemplateItemGear = TemplateItemGear });

            Context.SaveChanges();

            await RPGRepository.EquipGear(Guild, Unit.ID, Item.ID, new() { SlotGear.ID });


        }



    }
}