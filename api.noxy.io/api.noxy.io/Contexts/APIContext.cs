using api.noxy.io.Utilities;
using api.noxy.io.Models;
using api.noxy.io.Models.Auth;
using api.noxy.io.Models.Game.Guild;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace api.noxy.io.Context
{
    public class APIContext : DbContext
    {
        public DbSet<UserEntity> User => Set<UserEntity>();

        public DbSet<FeatEntity> Feat => Set<FeatEntity>();

        public DbSet<GuildEntity> Guild => Set<GuildEntity>();
        public DbSet<GuildRoleEntity> GuildRole => Set<GuildRoleEntity>();
        public DbSet<GuildFeatEntity> GuildFeat => Set<GuildFeatEntity>();

        public DbSet<GuildModifierEntity> GuildModifier => Set<GuildModifierEntity>();
        public DbSet<GuildRoleModifierEntity> GuildRoleModifier => Set<GuildRoleModifierEntity>();
        public DbSet<GuildUnitModifierEntity> GuildUnitModifier => Set<GuildUnitModifierEntity>();
        public DbSet<GuildMissionModifierEntity> GuildMissionModifier => Set<GuildMissionModifierEntity>();

        public DbSet<RoleEntity> Role => Set<RoleEntity>();
        public DbSet<RoleTypeEntity> RoleType => Set<RoleTypeEntity>();
        public DbSet<RoleLevelEntity> RoleLevel => Set<RoleLevelEntity>();

        public DbSet<UnitEntity> Unit => Set<UnitEntity>();

        public DbSet<MissionEntity> Mission => Set<MissionEntity>();
        public DbSet<MissionTypeEntity> MissionType => Set<MissionTypeEntity>();

        public DbSet<RequirementEntity> Requirement => Set<RequirementEntity>();


        public APIContext() { }

        public APIContext(DbContextOptions<APIContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<GuildModifierEntity>().UseTpcMappingStrategy();
            builder.Entity<RequirementEntity>().UseTpcMappingStrategy();
        }

        public void Seed()
        {

            var RequirementRoleAffinity = Requirement.Add(new() { ID = Guid.NewGuid() });
            var RequirementRoleProfession = Requirement.Add(new() { ID = Guid.NewGuid() });

            var RoleTypeAffinity = RoleType.Add(new() { Name = "Affinity" });
            var RoleTypeProfession = RoleType.Add(new() { Name = "Profession" });

            var FeatFirstGuild = Feat.Add(new() { Name = "First guild recruited" });
            var FeatFirstUnit = Feat.Add(new() { Name = "First unit recruited" });
            var FeatFirstMission = Feat.Add(new() { Name = "First mission completed" });
            var FeatFirstRole = Feat.Add(new() { Name = "First role unlocked" });
            var FeatFirstAdventure = Feat.Add(new() { Name = "First adventure completed" });

            Role.Add(new() { Name = "Cleric", RoleType = RoleTypeAffinity.Entity });
            Role.Add(new() { Name = "Knight", RoleType = RoleTypeAffinity.Entity });
            Role.Add(new() { Name = "Ranger", RoleType = RoleTypeAffinity.Entity });
            Role.Add(new() { Name = "Paladin", RoleType = RoleTypeAffinity.Entity, RequirementList = new() { RequirementRoleAffinity.Entity } });
            Role.Add(new() { Name = "Dark Knight", RoleType = RoleTypeAffinity.Entity, RequirementList = new() { RequirementRoleAffinity.Entity } });

            Role.Add(new() { Name = "Miner", RoleType = RoleTypeProfession.Entity });
            Role.Add(new() { Name = "Furrier", RoleType = RoleTypeProfession.Entity });
            Role.Add(new() { Name = "Blacksmith", RoleType = RoleTypeProfession.Entity });
            Role.Add(new() { Name = "Tailor", RoleType = RoleTypeProfession.Entity, RequirementList = new() { RequirementRoleProfession.Entity } });
            Role.Add(new() { Name = "Alchemist", RoleType = RoleTypeProfession.Entity, RequirementList = new() { RequirementRoleProfession.Entity } });
            Role.Add(new() { Name = "Woodcutter", RoleType = RoleTypeProfession.Entity, RequirementList = new() { RequirementRoleProfession.Entity } });

            GuildRoleModifier.Add(new() { Tag = GuildRoleModifierTagType.Count, Value = 1, ArithmeticalTag = ArithmeticalTagType.Additive, Feat = FeatFirstGuild.Entity });
            GuildRoleModifier.Add(new() { Tag = GuildRoleModifierTagType.Count, Value = 1, ArithmeticalTag = ArithmeticalTagType.Additive, Feat = FeatFirstGuild.Entity, RoleType = RoleTypeAffinity.Entity });
            GuildRoleModifier.Add(new() { Tag = GuildRoleModifierTagType.Count, Value = 1, ArithmeticalTag = ArithmeticalTagType.Additive, Feat = FeatFirstGuild.Entity, RoleType = RoleTypeProfession.Entity });
            GuildRoleModifier.Add(new() { Tag = GuildRoleModifierTagType.Experience, Value = 200, ArithmeticalTag = ArithmeticalTagType.Additive, Feat = FeatFirstGuild.Entity });
            GuildRoleModifier.Add(new() { Tag = GuildRoleModifierTagType.Experience, Value = 300, ArithmeticalTag = ArithmeticalTagType.Additive, Feat = FeatFirstGuild.Entity, RoleType = RoleTypeAffinity.Entity });
            GuildRoleModifier.Add(new() { Tag = GuildRoleModifierTagType.Experience, Value = 100, ArithmeticalTag = ArithmeticalTagType.Additive, Feat = FeatFirstGuild.Entity, RoleType = RoleTypeProfession.Entity });

            SaveChanges();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySQL();
        }

        public override int SaveChanges()
        {
            PerformEntityMaintenance();
            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            PerformEntityMaintenance();
            return await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken).ConfigureAwait(false);
        }

        private void PerformEntityMaintenance()
        {
            IEnumerable<EntityEntry> entries = ChangeTracker.Entries();
            foreach (EntityEntry entry in entries)
            {
                if (entry.Entity is SingleEntity entity)
                {
                    UpdateSingleEntityTimestamp(entity, entry.State);
                }
            }
        }

        private static void UpdateSingleEntityTimestamp(SingleEntity entity, EntityState state)
        {
            if (state == EntityState.Added)
            {
                entity.TimeCreated = DateTime.UtcNow;
            }
            else if (state == EntityState.Modified)
            {
                entity.TimeUpdated = DateTime.UtcNow;
            }
        }
    }
}
