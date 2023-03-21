using api.noxy.io.Models;
using api.noxy.io.Models.Game;
using api.noxy.io.Models.Game.Action;
using api.noxy.io.Models.Game.Effect;
using api.noxy.io.Models.Game.Modifier;
using api.noxy.io.Models.Game.Operation;
using api.noxy.io.Models.Game.Skill;
using api.noxy.io.Models.Game.Trigger;
using api.noxy.io.Models.Game.Unit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace api.noxy.io.Context
{
    public class APIContext : DbContext
    {
        public DbSet<UserEntity>? User { get; set; }
        public DbSet<GuildEntity>? Guild { get; set; }

        public DbSet<EffectEntity>? Effect { get; set; }
        public DbSet<OperationEntity>? Operation { get; set; }
        public DbSet<SkillEntity>? Skill { get; set; }
        public DbSet<UnitEntity>? Unit { get; set; }
        public DbSet<UnitClassEntity>? UnitClass { get; set; }

        public DbSet<ModifierEntity>? Modifier { get; set; }
        public DbSet<ArithmeticalModifierEntity>? ArithmeticalModifier { get; set; }
        public DbSet<AttributeModifierEntity>? AttributeModifier { get; set; }

        public DbSet<ActionEntity>? Action { get; set; }
        public DbSet<ComboPointActionEntity>? ComboPointAction { get; set; }
        public DbSet<DamageActionEntity>? DamageAction { get; set; }
        public DbSet<EffectActionEntity>? EffectAction { get; set; }
        public DbSet<HealActionEntity>? HealAction { get; set; }

        public DbSet<TriggerEntity>? Trigger { get; set; }
        public DbSet<DamageReceivedTriggerEntity>? DamageReceivedTrigger { get; set; }
        public DbSet<ExpirationTriggerEntity>? ExpirationTrigger { get; set; }
        public DbSet<HealingReceivedTriggerEntity>? HealingReceivedTrigger { get; set; }
        public DbSet<PeriodicTriggerEntity>? PeriodicTrigger { get; set; }

        public APIContext() { }

        public APIContext(DbContextOptions<APIContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<UserEntity>(UserEntity.AddTableToBuilder);
            modelBuilder.Entity<GuildEntity>(GuildEntity.AddTableToBuilder);

            modelBuilder.Entity<EffectEntity>(EffectEntity.AddTableToBuilder);
            modelBuilder.Entity<OperationEntity>(OperationEntity.AddTableToBuilder);
            modelBuilder.Entity<SkillEntity>(SkillEntity.AddTableToBuilder);
            modelBuilder.Entity<UnitEntity>(UnitEntity.AddTableToBuilder);
            modelBuilder.Entity<UnitClassEntity>(UnitClassEntity.AddTableToBuilder);

            modelBuilder.Entity<ModifierEntity>(ModifierEntity.AddTableToBuilder);
            modelBuilder.Entity<AttributeModifierEntity>(AttributeModifierEntity.AddTableToBuilder);
            modelBuilder.Entity<ArithmeticalModifierEntity>(ArithmeticalModifierEntity.AddTableToBuilder);

            modelBuilder.Entity<ActionEntity>(ActionEntity.AddTableToBuilder);
            modelBuilder.Entity<ComboPointActionEntity>(ComboPointActionEntity.AddTableToBuilder);
            modelBuilder.Entity<DamageActionEntity>(DamageActionEntity.AddTableToBuilder);
            modelBuilder.Entity<EffectActionEntity>(EffectActionEntity.AddTableToBuilder);
            modelBuilder.Entity<HealActionEntity>(HealActionEntity.AddTableToBuilder);

            modelBuilder.Entity<TriggerEntity>(TriggerEntity.AddTableToBuilder);
            modelBuilder.Entity<DamageReceivedTriggerEntity>(DamageReceivedTriggerEntity.AddTableToBuilder);
            modelBuilder.Entity<ExpirationTriggerEntity>(ExpirationTriggerEntity.AddTableToBuilder);
            modelBuilder.Entity<HealingReceivedTriggerEntity>(HealingReceivedTriggerEntity.AddTableToBuilder);
            modelBuilder.Entity<PeriodicTriggerEntity>(PeriodicTriggerEntity.AddTableToBuilder);
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
