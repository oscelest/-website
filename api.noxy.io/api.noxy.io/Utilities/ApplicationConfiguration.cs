namespace Database.Utilities
{
    public interface IApplicationConfiguration
    {
        public ApplicationConfiguration.JWTConfiguration JWT { get; }
        public ApplicationConfiguration.CORSConfiguration CORS { get; }
        public ApplicationConfiguration.GameConfiguration Game { get; }
        public ApplicationConfiguration.DatabaseConfiguration Database { get; }
    }

    public class ApplicationConfiguration : IApplicationConfiguration
    {
        public JWTConfiguration JWT { get; }
        public CORSConfiguration CORS { get; }
        public GameConfiguration Game { get; }
        public DatabaseConfiguration Database { get; }

        public ApplicationConfiguration(IConfiguration configuration)
        {
            JWT = new JWTConfiguration(configuration.GetSection("JWT"));
            CORS = new CORSConfiguration(configuration.GetSection("CORS"));
            Game = new GameConfiguration(configuration.GetSection("Game"));
            Database = new DatabaseConfiguration(configuration.GetSection("Database"));
        }

        public class JWTConfiguration
        {
            public string Secret { get; }
            public string Issuer { get; }
            public string Audience { get; }

            public JWTConfiguration(IConfigurationSection? section)
            {
                if (section == null) throw new ArgumentNullException("Database");

                Secret = section.GetSection("Secret")?.Get<string>() ?? throw new ArgumentNullException("JWT:Secret");
                Issuer = section.GetSection("Issuer")?.Get<string>() ?? throw new ArgumentNullException("JWT:Issuer");
                Audience = section.GetSection("Audience")?.Get<string>() ?? throw new ArgumentNullException("JWT:Audience");
            }
        }

        public class CORSConfiguration
        {
            public string[] Origins { get; }
            public string[] Headers { get; }
            public string[] Methods { get; }

            public CORSConfiguration(IConfigurationSection? section)
            {
                if (section != null)
                {
                    Origins = section?.GetSection("Origins")?.Get<string[]>() ?? new string[] { "*" };
                    Headers = section?.GetSection("Headers")?.Get<string[]>() ?? new string[] { "*" };
                    Methods = section?.GetSection("Methods")?.Get<string[]>() ?? new string[] { "*" };
                }
                else
                {
                    Origins = new string[] { "*" };
                    Headers = new string[] { "*" };
                    Methods = new string[] { "*" };
                }
            }
        }

        public class GameConfiguration
        {
            public int UnitListCount { get; }
            public int UnitListRefresh { get; }
            public int MissionListCount { get; }
            public int MissionListRefresh { get; }
            public float MissionListRoleRatio { get; }

            public GameConfiguration(IConfigurationSection? section)
            {
                if (section == null) throw new ArgumentNullException("Game");

                UnitListCount = section.GetSection("UnitListCount")?.Get<int>() ?? throw new ArgumentNullException("Game:UnitListCount");
                UnitListRefresh = section.GetSection("UnitListRefresh")?.Get<int>() ?? throw new ArgumentNullException("Game:UnitListRefresh");
                MissionListCount = section.GetSection("MissionListCount")?.Get<int>() ?? throw new ArgumentNullException("Game:MissionListCount");
                MissionListRefresh = section.GetSection("MissionListRefresh")?.Get<int>() ?? throw new ArgumentNullException("Game:MissionListRefresh");
                MissionListRoleRatio = section.GetSection("MissionListRoleRatio")?.Get<float>() ?? throw new ArgumentNullException("Game:MissionListRoleRatio");
            }
        }

        public class DatabaseConfiguration
        {
            public string ConnectionString { get; }

            public DatabaseConfiguration(IConfigurationSection? section)
            {
                if (section == null) throw new ArgumentNullException("Database");

                ConnectionString = section.GetSection("ConnectionString")?.Get<string>() ?? throw new ArgumentNullException("Database:ConnectionString");
            }
        }


    }
}
