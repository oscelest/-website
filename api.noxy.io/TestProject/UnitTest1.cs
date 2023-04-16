using api.noxy.io.Context;
using api.noxy.io.Interface;
using api.noxy.io.Models.RPG;
using api.noxy.io.Utilities;
using api.noxy.io.Utility;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Org.BouncyCastle.Crypto.Engines;

namespace TestProject
{
    [TestClass]
    public class UnitTest1
    {
        private readonly RPGContext Context;
        private readonly IRPGRepository RPGRepository;
        private readonly IAuthRepository AuthRepository;
        private readonly User User;
        private readonly Guild Guild;

        public UnitTest1()
        {
            var config = new ConfigurationBuilder().AddJsonFile("appsettings.json", optional: true, reloadOnChange: true).Build();
            var appconfig = new ApplicationConfiguration(config);

            var services = new ServiceCollection();
            services.AddScoped<IConfiguration>(_ => config);
            services.AddTransient<IApplicationConfiguration, ApplicationConfiguration>();
            services.AddDbContext<RPGContext>(options => options.UseMySQL(appconfig.Database.ConnectionString));
            services.AddTransient<IRPGRepository, RPGRepository>();
            services.AddTransient<IAuthRepository, AuthRepository>();

            var serviceProvider = services.BuildServiceProvider();
            Context = serviceProvider.GetRequiredService<RPGContext>();
            RPGRepository = serviceProvider.GetRequiredService<IRPGRepository>();
            AuthRepository = serviceProvider.GetRequiredService<IAuthRepository>();

            var dbContext = serviceProvider.GetRequiredService<RPGContext>();
            Context.Database.EnsureDeleted();
            Context.Database.EnsureCreated();

            var salt = User.GenerateSalt();
            var hash = User.GenerateHash("Test1234", salt);

            User = Context.User.Add(new() { Email = "test@example.com", Salt = salt, Hash = hash }).Entity;
            Guild = Context.Guild.Add(new() { Name = "Test", User = User }).Entity;

            // Create another guild to make sure queries only affect the main guild
            var entityUser = Context.User.Add(new() { Email = "placeholder@example.com", Salt = salt, Hash = hash }).Entity;
            var entityGuild = Context.Guild.Add(new() { Name = "Placeholder", User = User }).Entity;

            var entityTemplateItemMaterialIronOre = Context.TemplateItemMaterial.Add(new() { Name = "Iron Ore" }).Entity;
            var entityTemplateItemMaterialHolyEssence = Context.TemplateItemMaterial.Add(new() { Name = "Holy Essence" }).Entity;

            var entityTemplateSlotWeapon = Context.TemplateSlot.Add(new() { Name = "Weapon" }).Entity;

            var entityTemplateItemEquipmentIronSword = Context.TemplateItemEquipment.Add(new() { Name = "Iron Sword", TemplateSlot = entityTemplateSlotWeapon }).Entity;
            var entityTemplateItemEquipmentHolyIronSword = Context.TemplateItemEquipment.Add(new() { Name = "Holy Iron Sword", TemplateSlot = entityTemplateSlotWeapon }).Entity;

            var entityTemplateRecipeIronSword = Context.TemplateRecipe.Add(new() { Name = "Recipe: Iron Sword" }).Entity;
            Context.VolumeItemRecipe.Add(new() { TemplateRecipe = entityTemplateRecipeIronSword, Component = true, TemplateItem = entityTemplateItemMaterialIronOre, Count = 5 });
            Context.VolumeItemRecipe.Add(new() { TemplateRecipe = entityTemplateRecipeIronSword, Component = false, TemplateItem = entityTemplateItemEquipmentIronSword, Count = 1 });

            var entityTemplateRecipeHolyIronSword = Context.TemplateRecipe.Add(new() { Name = "Recipe: Holy Iron Sword" }).Entity;
            Context.VolumeItemRecipe.Add(new() { TemplateRecipe = entityTemplateRecipeHolyIronSword, Component = true, TemplateItem = entityTemplateItemMaterialHolyEssence, Count = 2 });
            Context.VolumeItemRecipe.Add(new() { TemplateRecipe = entityTemplateRecipeHolyIronSword, Component = true, TemplateItem = entityTemplateItemEquipmentIronSword, Count = 1 });
            Context.VolumeItemRecipe.Add(new() { TemplateRecipe = entityTemplateRecipeHolyIronSword, Component = false, TemplateItem = entityTemplateItemEquipmentHolyIronSword, Count = 1 });

            Context.ItemMaterial.Add(new ItemMaterial() { Guild = Guild, TemplateItem = entityTemplateItemMaterialIronOre, Count = 5 });
            Context.ItemMaterial.Add(new ItemMaterial() { Guild = entityGuild, TemplateItem = entityTemplateItemMaterialIronOre, Count = 5 });

            Context.SaveChanges();
        }

        [TestMethod]
        public async Task LoadGuildSuccessTest()
        {
            var entityGuild = await RPGRepository.LoadGuild(User);
            Assert.IsNotNull(entityGuild);
            Assert.AreEqual(entityGuild.Name, "Test");
            Assert.AreEqual(entityGuild.User.Email, "test@example.com");
        }

        public async Task CraftItem()
        {

        }
    }
}