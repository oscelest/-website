using api.noxy.io.Context;
using api.noxy.io.Exceptions;
using api.noxy.io.Interface;
using api.noxy.io.Models.RPG;
using api.noxy.io.Utilities;
using api.noxy.io.Utility;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Org.BouncyCastle.Crypto.Engines;
using System.Reflection;

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
            Assert.AreEqual(entityGuild.Name, "Test");
            Assert.AreEqual(entityGuild.User.Email, "test@example.com");
        }

        [TestMethod]
        public async Task CraftItemNoRecipeUnlocked()
        {
            TemplateRecipe TemplateRecipe = Context.TemplateRecipe.Add(new() { Name = "Recipe: Equipment" }).Entity;
            Context.SaveChanges();

            try
            {
                await RPGRepository.CraftItem(Guild, TemplateRecipe.ID, new List<Guid>() { });
                Assert.Fail("Guild should not have access to this recipe");
            }
            catch (EntityNotFoundException<UnlockableRecipe> ex)
            {
                Type type = ex.Identifier.GetType();
                Guid? GuildID = (Guid?)type.GetProperty("Guild")?.GetValue(ex.Identifier, null);
                Guid? RecipeID = (Guid?)type.GetProperty("Recipe")?.GetValue(ex.Identifier, null);

                Assert.AreEqual(GuildID, Guild.ID);
                Assert.AreEqual(RecipeID, TemplateRecipe.ID);
            }
        }


        [TestMethod]
        public async Task CraftItemWrongMaterial()
        {
            TemplateItemMaterial TemplateItemMaterialSuccess = Context.TemplateItemMaterial.Add(new() { Name = "Success Material" }).Entity;
            TemplateItemMaterial TemplateItemMaterialFailure = Context.TemplateItemMaterial.Add(new() { Name = "Failure Material" }).Entity;

            TemplateRecipe TemplateRecipe = Context.TemplateRecipe.Add(new() { Name = "Recipe: Equipment" }).Entity;
            Context.UnlockableRecipe.Add(new() { Guild = Guild, TemplateRecipe = TemplateRecipe });
            Context.VolumeItemRecipe.Add(new() { TemplateRecipe = TemplateRecipe, Component = true, TemplateItem = TemplateItemMaterialSuccess, Count = 5 });

            ItemMaterial ItemMaterial = Context.ItemMaterial.Add(new ItemMaterial() { Guild = Guild, TemplateItem = TemplateItemMaterialFailure, Count = 5 }).Entity;

            Context.SaveChanges();

            try
            {
                await RPGRepository.CraftItem(Guild, TemplateRecipe.ID, new List<Guid>() { ItemMaterial.ID });
            }
            catch (EntityNotFoundException<Item> ex)
            {
                Type type = ex.Identifier.GetType();
                Guid? GuildID = (Guid?)type.GetProperty("Guild")?.GetValue(ex.Identifier, null);
                Guid? TemplateItemID = (Guid?)type.GetProperty("TemplateItem")?.GetValue(ex.Identifier, null);

                Assert.AreEqual(GuildID, Guild.ID);
                Assert.AreEqual(TemplateItemID, TemplateItemMaterialSuccess.ID);
            }
        }

        [TestMethod]
        public async Task CraftItemInsufficientMaterial()
        {
            TemplateItemMaterial TemplateItemMaterial = Context.TemplateItemMaterial.Add(new() { Name = "Material" }).Entity;

            TemplateRecipe TemplateRecipe = Context.TemplateRecipe.Add(new() { Name = "Recipe: Equipment" }).Entity;
            Context.UnlockableRecipe.Add(new() { Guild = Guild, TemplateRecipe = TemplateRecipe });
            Context.VolumeItemRecipe.Add(new() { TemplateRecipe = TemplateRecipe, Component = true, TemplateItem = TemplateItemMaterial, Count = 5 });

            ItemMaterial ItemMaterial = Context.ItemMaterial.Add(new ItemMaterial() { Guild = Guild, TemplateItem = TemplateItemMaterial, Count = 2 }).Entity;

            Context.SaveChanges();

            try
            {
                await RPGRepository.CraftItem(Guild, TemplateRecipe.ID, new List<Guid>() { ItemMaterial.ID });
            }
            catch (EntityConditionException<Item> ex)
            {
                Assert.AreEqual((Guid)ex.Identifier, ItemMaterial.ID);
            }
        }

        [TestMethod]
        public async Task CraftItemFromMaterial()
        {
            TemplateSlot TemplateSlotEquipment = Context.TemplateSlot.Add(new() { Name = "Equipment Slot" }).Entity;
            TemplateItemMaterial TemplateItemMaterial = Context.TemplateItemMaterial.Add(new() { Name = "Material" }).Entity;
            TemplateItemEquipment TemplateItemEquipment = Context.TemplateItemEquipment.Add(new() { Name = "Equipment", TemplateSlot = TemplateSlotEquipment }).Entity;

            TemplateRecipe TemplateRecipe = Context.TemplateRecipe.Add(new() { Name = "Recipe: Equipment" }).Entity;
            Context.UnlockableRecipe.Add(new() { Guild = Guild, TemplateRecipe = TemplateRecipe });
            Context.VolumeItemRecipe.Add(new() { TemplateRecipe = TemplateRecipe, Component = true, TemplateItem = TemplateItemMaterial, Count = 5 });
            Context.VolumeItemRecipe.Add(new() { TemplateRecipe = TemplateRecipe, Component = false, TemplateItem = TemplateItemEquipment, Count = 1 });

            ItemMaterial ItemMaterial = Context.ItemMaterial.Add(new ItemMaterial() { Guild = Guild, TemplateItem = TemplateItemMaterial, Count = 5 }).Entity;

            Context.SaveChanges();

            await RPGRepository.CraftItem(Guild, TemplateRecipe.ID, new List<Guid>() { ItemMaterial.ID });

            ItemEquipment? EquipmentResult = await Context.ItemEquipment.FirstOrDefaultAsync(x => x.TemplateItem.ID == TemplateItemEquipment.ID && x.Guild.ID == Guild.ID);
            Assert.IsNotNull(EquipmentResult);

            ItemMaterial? MaterialResult = await Context.ItemMaterial.FirstOrDefaultAsync(x => x.TemplateItem.ID == TemplateItemMaterial.ID && x.Guild.ID == Guild.ID);
            Assert.IsNotNull(MaterialResult);
            Assert.AreEqual(MaterialResult.Count, 0);
        }


        [TestMethod]
        public async Task CraftItemFromMaterialAndEquipment()
        {
            TemplateSlot TemplateSlotEquipment = Context.TemplateSlot.Add(new() { Name = "Equipment Slot" }).Entity;

            TemplateItemMaterial TemplateItemMaterial = Context.TemplateItemMaterial.Add(new() { Name = "Material" }).Entity;
            TemplateItemEquipment TemplateItemEquipment = Context.TemplateItemEquipment.Add(new() { Name = "Equipment", TemplateSlot = TemplateSlotEquipment }).Entity;
            TemplateItemEquipment TemplateItemResult = Context.TemplateItemEquipment.Add(new() { Name = "Result", TemplateSlot = TemplateSlotEquipment }).Entity;

            TemplateRecipe TemplateRecipe = Context.TemplateRecipe.Add(new() { Name = "Recipe: Equipment" }).Entity;
            Context.UnlockableRecipe.Add(new() { Guild = Guild, TemplateRecipe = TemplateRecipe });
            Context.VolumeItemRecipe.Add(new() { TemplateRecipe = TemplateRecipe, Component = true, TemplateItem = TemplateItemMaterial, Count = 5 });
            Context.VolumeItemRecipe.Add(new() { TemplateRecipe = TemplateRecipe, Component = true, TemplateItem = TemplateItemEquipment, Count = 1 });
            Context.VolumeItemRecipe.Add(new() { TemplateRecipe = TemplateRecipe, Component = false, TemplateItem = TemplateItemResult, Count = 1 });

            ItemMaterial ItemMaterial = Context.ItemMaterial.Add(new ItemMaterial() { Guild = Guild, TemplateItem = TemplateItemMaterial, Count = 5 }).Entity;
            ItemEquipment ItemEquipment = Context.ItemEquipment.Add(new ItemEquipment() { Guild = Guild, TemplateItem = TemplateItemEquipment }).Entity;

            Context.SaveChanges();

            await RPGRepository.CraftItem(Guild, TemplateRecipe.ID, new List<Guid>() { ItemMaterial.ID, ItemEquipment.ID });
        }
    }
}