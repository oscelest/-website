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

        public APIContext() { }

        public APIContext(DbContextOptions<APIContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<GuildModifierEntity>().UseTpcMappingStrategy();
            builder.Entity<RequirementEntity>().UseTpcMappingStrategy();
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
