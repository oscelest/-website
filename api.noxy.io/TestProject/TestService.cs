using api.noxy.io.Context;
using api.noxy.io.Interface;
using api.noxy.io.Models.RPG;
using api.noxy.io.Utility;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace TestProject
{
    [TestClass]
    public class TestService
    {
        private static readonly ServiceProvider ServiceProvider = GetServiceProvider();

        public static RPGContext Context { get; } = ServiceProvider.GetRequiredService<RPGContext>();
        public static IRPGRepository RPGRepository { get; } = ServiceProvider.GetRequiredService<IRPGRepository>();
        public static IAuthRepository AuthRepository { get; } = ServiceProvider.GetRequiredService<IAuthRepository>();

        [AssemblyInitialize]
        public static void MyTestInitialize()
        {
            Context.Database.EnsureDeleted();
            Context.Database.EnsureCreated();
        }

        private static ServiceProvider GetServiceProvider()
        {
            IConfigurationRoot config = new ConfigurationBuilder().AddJsonFile("appsettings.json", optional: true, reloadOnChange: true).Build();
            ApplicationConfiguration appconfig = new(config);

            ServiceCollection services = new();
            services.AddScoped<IConfiguration>(_ => config);
            services.AddTransient<IApplicationConfiguration, ApplicationConfiguration>();
            services.AddDbContext<RPGContext>(options => options.UseMySQL(appconfig.Database.ConnectionString));
            services.AddTransient<IRPGRepository, RPGRepository>();
            services.AddTransient<IAuthRepository, AuthRepository>();
            return services.BuildServiceProvider();
        }

        public static User GetNewUser()
        {
            byte[] salt = User.GenerateSalt();
            byte[] hash = User.GenerateHash("Test1234", salt);
            return Context.User.Add(new() { Email = $"{Guid.NewGuid()}@test.com", Salt = salt, Hash = hash }).Entity;
        }

        public static Guild GetNewGuild()
        {
            return Context.Guild.Add(new() { Name = "Test", User = GetNewUser() }).Entity;
        }
    }
}