using api.noxy.io.Context;
using api.noxy.io.Interface;
using api.noxy.io.Utility;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace TestProject
{
    [TestClass]
    public class UnitTest1
    {
        private readonly RPGContext db;
        private readonly IJWT jwt;
        private readonly IApplicationConfiguration config;
        private readonly IRPGRepository rpg;

        public UnitTest1()
        {
            ApplicationConfiguration appconfig = new(new ConfigurationBuilder().AddJsonFile("appsettings.json", optional: true, reloadOnChange: true).Build());

            var services = new ServiceCollection();
            services.AddDbContext<RPGContext>(options => options.UseMySQL(appconfig.Database.ConnectionString));
            services.AddTransient<IJWT, JWT>();
            services.AddTransient<IApplicationConfiguration, ApplicationConfiguration>();
            services.AddTransient<IRPGRepository, RPGRepository>();

            var serviceProvider = services.BuildServiceProvider();
            db = serviceProvider.GetService<RPGContext>()!;
            jwt = serviceProvider.GetService<IJWT>()!;
            config = serviceProvider.GetService<IApplicationConfiguration>()!;
            rpg = serviceProvider.GetService<IRPGRepository>()!;
        }

        [TestMethod]
        public void TestMethod1()
        {
            Assert.IsTrue(true);
        }
    }
}