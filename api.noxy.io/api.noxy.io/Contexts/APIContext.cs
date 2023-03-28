using api.noxy.io.Models;
using api.noxy.io.Models.Game.Guild;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace api.noxy.io.Context
{
    public class APIContext : DbContext
    {
        public DbSet<UserEntity> User => Set<UserEntity>();

        public DbSet<FeatEntity> Feat => Set<FeatEntity>();
        public DbSet<FeatRequirementEntity> FeatRequirement => Set<FeatRequirementEntity>();
        public DbSet<FeatStatisticRequirementEntity> FeatStatisticRequirement => Set<FeatStatisticRequirementEntity>();

        public DbSet<GuildEntity> Guild => Set<GuildEntity>();
        public DbSet<GuildModifierEntity> GuildModifier => Set<GuildModifierEntity>();
        public DbSet<GuildRoleModifierEntity> GuildRoleModifier => Set<GuildRoleModifierEntity>();
        public DbSet<GuildUnitModifierEntity> GuildUnitModifier => Set<GuildUnitModifierEntity>();
        public DbSet<GuildMissionModifierEntity> GuildMissionModifier => Set<GuildMissionModifierEntity>();

        public DbSet<StatisticEntity> Statistic => Set<StatisticEntity>();

        public DbSet<RoleEntity> Role => Set<RoleEntity>();
        public DbSet<RoleTypeEntity> RoleType => Set<RoleTypeEntity>();
        public DbSet<RoleLevelEntity> RoleLevel => Set<RoleLevelEntity>();
            
        public DbSet<UnitEntity> Unit => Set<UnitEntity>();

        public DbSet<MissionEntity> Mission => Set<MissionEntity>();
        public DbSet<MissionTypeEntity> MissionType => Set<MissionTypeEntity>();
        public DbSet<MissionStatisticEntity> MissionStatistic => Set<MissionStatisticEntity>();


        //public DbSet<SkillEntity> Skill { get; set; }
        //public DbSet<EffectEntity> Effect { get; set; }
        //public DbSet<OperationEntity> Operation { get; set; }

        //public DbSet<ModifierEntity> Modifier { get; set; }
        //public DbSet<ArithmeticalModifierEntity> ArithmeticalModifier { get; set; }
        //public DbSet<AttributeModifierEntity> AttributeModifier { get; set; }

        //public DbSet<ActionEntity> Action { get; set; }
        //public DbSet<ComboPointActionEntity> ComboPointAction { get; set; }
        //public DbSet<DamageActionEntity> DamageAction { get; set; }
        //public DbSet<EffectActionEntity> EffectAction { get; set; }
        //public DbSet<HealActionEntity> HealAction { get; set; }

        //public DbSet<TriggerEntity> Trigger { get; set; }
        //public DbSet<DamageReceivedTriggerEntity> DamageReceivedTrigger { get; set; }
        //public DbSet<ExpirationTriggerEntity> ExpirationTrigger { get; set; }
        //public DbSet<HealingReceivedTriggerEntity> HealingReceivedTrigger { get; set; }
        //public DbSet<PeriodicTriggerEntity> PeriodicTrigger { get; set; }

        public APIContext() { }

        public APIContext(DbContextOptions<APIContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<GuildEntity>().HasMany(x => x.FeatList).WithMany(x => x.GuildList).UsingEntity(x => x.ToTable("$Guild-Feat"));
            builder.Entity<RoleEntity>().HasMany(x => x.FeatList).WithMany(x => x.RoleList).UsingEntity(x => x.ToTable("$Role-Feat"));

            builder.Entity<GuildModifierEntity>().UseTpcMappingStrategy();
            builder.Entity<FeatRequirementEntity>().UseTpcMappingStrategy();
            builder.Entity<StatisticEntity>().UseTpcMappingStrategy();
            //.ToTable("GuildModifier")
            //.HasDiscriminator(x => x.EntityType)
            //.HasValue<GuildRoleModifierEntity>(GuildModifierEntityType.Role)
            //.HasValue<GuildUnitModifierEntity>(GuildModifierEntityType.Unit);

            //builder.Entity<SkillEntity>(SkillEntity.AddTableToBuilder);
            //builder.Entity<EffectEntity>(EffectEntity.AddTableToBuilder);
            //builder.Entity<OperationEntity>(OperationEntity.AddTableToBuilder);

            //builder.Entity<ModifierEntity>(ModifierEntity.AddTableToBuilder);
            //builder.Entity<AttributeModifierEntity>(AttributeModifierEntity.AddTableToBuilder);
            //builder.Entity<ArithmeticalModifierEntity>(ArithmeticalModifierEntity.AddTableToBuilder);

            //builder.Entity<ActionEntity>(ActionEntity.AddTableToBuilder);
            //builder.Entity<ComboPointActionEntity>(ComboPointActionEntity.AddTableToBuilder);
            //builder.Entity<DamageActionEntity>(DamageActionEntity.AddTableToBuilder);
            //builder.Entity<EffectActionEntity>(EffectActionEntity.AddTableToBuilder);
            //builder.Entity<HealActionEntity>(HealActionEntity.AddTableToBuilder);

            //builder.Entity<TriggerEntity>(TriggerEntity.AddTableToBuilder);
            //builder.Entity<DamageReceivedTriggerEntity>(DamageReceivedTriggerEntity.AddTableToBuilder);
            //builder.Entity<ExpirationTriggerEntity>(ExpirationTriggerEntity.AddTableToBuilder);
            //builder.Entity<HealingReceivedTriggerEntity>(HealingReceivedTriggerEntity.AddTableToBuilder);
            //builder.Entity<PeriodicTriggerEntity>(PeriodicTriggerEntity.AddTableToBuilder);
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
                if (entry.Entity is SimpleEntity entity)
                {
                    UpdateSimpleEntityTimestamp(entity, entry.State);
                }
            }
        }

        private static void UpdateSimpleEntityTimestamp(SimpleEntity entity, EntityState state)
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
