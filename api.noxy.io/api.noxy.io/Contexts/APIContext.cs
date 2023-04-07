using api.noxy.io.Utilities;
using api.noxy.io.Models;
using api.noxy.io.Models.Auth;
using api.noxy.io.Models.Game.Guild;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using api.noxy.io.Models.Game.Item;

namespace api.noxy.io.Context
{
    public class APIContext : DbContext
    {
        public DbSet<UserEntity> User => Set<UserEntity>();

        public DbSet<FeatEntity> Feat => Set<FeatEntity>();

        public DbSet<GuildEntity> Guild => Set<GuildEntity>();
        public DbSet<GuildRoleEntity> GuildRole => Set<GuildRoleEntity>();
        public DbSet<GuildFeatEntity> GuildFeat => Set<GuildFeatEntity>();
        public DbSet<GuildItemEntity> GuildItem => Set<GuildItemEntity>();

        public DbSet<GuildModifierEntity> GuildModifier => Set<GuildModifierEntity>();
        public DbSet<GuildRoleModifierEntity> GuildRoleModifier => Set<GuildRoleModifierEntity>();
        public DbSet<GuildUnitModifierEntity> GuildUnitModifier => Set<GuildUnitModifierEntity>();
        public DbSet<GuildMissionModifierEntity> GuildMissionModifier => Set<GuildMissionModifierEntity>();

        public DbSet<RoleEntity> Role => Set<RoleEntity>();
        public DbSet<RoleTypeEntity> RoleType => Set<RoleTypeEntity>();

        public DbSet<UnitEntity> Unit => Set<UnitEntity>();
        public DbSet<UnitRoleEntity> UnitRole => Set<UnitRoleEntity>();
        public DbSet<UnitTypeEntity> UnitType => Set<UnitTypeEntity>();

        public DbSet<MissionEntity> Mission => Set<MissionEntity>();
        public DbSet<MissionTypeEntity> MissionType => Set<MissionTypeEntity>();

        public DbSet<RequirementEntity> Requirement => Set<RequirementEntity>();

        public DbSet<ItemEntity> Item => Set<ItemEntity>();
        public DbSet<ItemSocketEntity> ItemSocket => Set<ItemSocketEntity>();
        
        public DbSet<MaterialItemEntity> MaterialItem => Set<MaterialItemEntity>();

        public DbSet<EquipmentItemEntity> EquipmentItem => Set<EquipmentItemEntity>();
        public DbSet<EquipmentSlotEntity> EquipmentSlot => Set<EquipmentSlotEntity>();

        public DbSet<SocketEntity> Socket => Set<SocketEntity>();

        public DbSet<RecipeEntity> Recipe => Set<RecipeEntity>();
        public DbSet<RecipeItemEntity> RecipeItem => Set<RecipeItemEntity>();


        public APIContext() { }

        public APIContext(DbContextOptions<APIContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ItemEntity>().UseTpcMappingStrategy();
            builder.Entity<RequirementEntity>().UseTpcMappingStrategy();
            builder.Entity<GuildModifierEntity>().UseTpcMappingStrategy();
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

            var EquipmentSlotWeapon = EquipmentSlot.Add(new() { Name = "Weapon slot" });

            var UnitTypeHuman = UnitType.Add(new() { Name = "Human", EquipmentSlotList = new() { EquipmentSlotWeapon.Entity } });
            var UnitTypeElf = UnitType.Add(new() { Name = "Elf", EquipmentSlotList = new() { EquipmentSlotWeapon.Entity } });

            var SocketMasterwork = Socket.Add(new() { Name = "Masterwork", Description = "Improvements related to physical offense and defense." });

            var EquipmentIronSword = EquipmentItem.Add(new() { Name = "Iron Sword", Stackable = false, Slot = EquipmentSlotWeapon.Entity });
            ItemSocket.Add(new() { Item = EquipmentIronSword.Entity, Socket = SocketMasterwork.Entity });



            // Polished, annealed, tempered, refined, honed, mastercrafted
            // Enchanted, enhanced, bolstered, enveloped, inspired, interpolated, infused, empowered, unleashed, 
            // Runed, shaped, 
            // Cursed, Malevolent, Blasphemous, Hexed, Occult
            // Blessed, Radiant, Angelic, Seraphic, Godly, Saintly, Sanctified, Hallowed, Consecrated, Beatified, Anointed
            // Carved, engraved, 
            // Chiseled, etched, engraved, embedded, inscribed, lettered, calligraphic

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
