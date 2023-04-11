using api.noxy.io.Utilities;
using api.noxy.io.Models;
using api.noxy.io.Models.Auth;
using api.noxy.io.Models.Game.Guild;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using api.noxy.io.Models.Game.Item;
using api.noxy.io.Models.Game.Socket;
using api.noxy.io.Models.Game.Recipe;
using api.noxy.io.Models.Game.Requirement;
using api.noxy.io.Models.Game.Mission;
using api.noxy.io.Models.Game.Role;
using api.noxy.io.Models.Game.Feat;
using api.noxy.io.Models.Game.Unit;

namespace api.noxy.io.Context
{
    public class APIContext : DbContext
    {
        public DbSet<UserEntity> User => Set<UserEntity>();

        public DbSet<FeatEntity> Feat => Set<FeatEntity>();

        public DbSet<GuildEntity> Guild => Set<GuildEntity>();
        public DbSet<GuildRoleEntity> GuildRole => Set<GuildRoleEntity>();
        public DbSet<GuildFeatEntity> GuildFeat => Set<GuildFeatEntity>();
        public DbSet<GuildRecipeEntity> GuildRecipe => Set<GuildRecipeEntity>();

        public DbSet<GuildUnitEntity> GuildUnit => Set<GuildUnitEntity>();
        public DbSet<GuildItemEntity<ItemEntity>> GuildItem => Set<GuildItemEntity<ItemEntity>>();
        public DbSet<GuildEquipmentItemEntity> GuildEquipmentItem => Set<GuildEquipmentItemEntity>();
        public DbSet<GuildMaterialItemEntity> GuildMaterialItem => Set<GuildMaterialItemEntity>();

        public DbSet<GuildModifierEntity> GuildModifier => Set<GuildModifierEntity>();
        public DbSet<GuildRoleModifierEntity> GuildRoleModifier => Set<GuildRoleModifierEntity>();
        public DbSet<GuildUnitModifierEntity> GuildUnitModifier => Set<GuildUnitModifierEntity>();
        public DbSet<GuildMissionModifierEntity> GuildMissionModifier => Set<GuildMissionModifierEntity>();

        public DbSet<RoleEntity> Role => Set<RoleEntity>();
        public DbSet<RoleTypeEntity> RoleType => Set<RoleTypeEntity>();

        public DbSet<UnitEntity> Unit => Set<UnitEntity>();
        public DbSet<UnitTypeEntity> UnitType => Set<UnitTypeEntity>();
        public DbSet<UnitRoleEntity> UnitRole => Set<UnitRoleEntity>();

        public DbSet<MissionEntity> Mission => Set<MissionEntity>();

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
            builder.Entity<GuildItemEntity<ItemEntity>>().UseTpcMappingStrategy();
        }

        public void Seed()
        {

            var RequirementRoleAdventuring = Requirement.Add(new() { });
            var RequirementRoleGathering = Requirement.Add(new() { });
            var RequirementRoleCrafting = Requirement.Add(new() { });

            var RoleTypeAdventuring = RoleType.Add(new() { Name = "Adventuring" });
            var RoleTypeGathering = RoleType.Add(new() { Name = "Gathering" });

            var FeatFirstGuild = Feat.Add(new() { Name = "First guild recruited" });
            var FeatFirstUnit = Feat.Add(new() { Name = "First unit recruited" });
            var FeatFirstMission = Feat.Add(new() { Name = "First mission completed" });
            var FeatFirstRole = Feat.Add(new() { Name = "First role unlocked" });
            var FeatFirstAdventure = Feat.Add(new() { Name = "First adventure completed" });

            Role.Add(new() { Name = "Cleric", RoleType = RoleTypeAdventuring.Entity });
            Role.Add(new() { Name = "Knight", RoleType = RoleTypeAdventuring.Entity });
            Role.Add(new() { Name = "Ranger", RoleType = RoleTypeAdventuring.Entity });
            Role.Add(new() { Name = "Paladin", RoleType = RoleTypeAdventuring.Entity, RequirementList = new() { RequirementRoleAdventuring.Entity } });
            Role.Add(new() { Name = "Dark Knight", RoleType = RoleTypeAdventuring.Entity, RequirementList = new() { RequirementRoleAdventuring.Entity } });

            Role.Add(new() { Name = "Miner", RoleType = RoleTypeGathering.Entity });
            Role.Add(new() { Name = "Furrier", RoleType = RoleTypeGathering.Entity });
            Role.Add(new() { Name = "Herbalist", RoleType = RoleTypeGathering.Entity });
            Role.Add(new() { Name = "Woodcutter", RoleType = RoleTypeGathering.Entity, RequirementList = new() { RequirementRoleGathering.Entity } });

            GuildRoleModifier.Add(new() { Tag = ModifierGuildRoleTagType.Count, Value = 1, ArithmeticalTag = ArithmeticalTagType.Additive, Feat = FeatFirstGuild.Entity });
            GuildRoleModifier.Add(new() { Tag = ModifierGuildRoleTagType.Count, Value = 1, ArithmeticalTag = ArithmeticalTagType.Additive, Feat = FeatFirstGuild.Entity, RoleType = RoleTypeAdventuring.Entity });
            GuildRoleModifier.Add(new() { Tag = ModifierGuildRoleTagType.Count, Value = 1, ArithmeticalTag = ArithmeticalTagType.Additive, Feat = FeatFirstGuild.Entity, RoleType = RoleTypeGathering.Entity });
            GuildRoleModifier.Add(new() { Tag = ModifierGuildRoleTagType.Experience, Value = 200, ArithmeticalTag = ArithmeticalTagType.Additive, Feat = FeatFirstGuild.Entity });
            GuildRoleModifier.Add(new() { Tag = ModifierGuildRoleTagType.Experience, Value = 300, ArithmeticalTag = ArithmeticalTagType.Additive, Feat = FeatFirstGuild.Entity, RoleType = RoleTypeAdventuring.Entity });
            GuildRoleModifier.Add(new() { Tag = ModifierGuildRoleTagType.Experience, Value = 100, ArithmeticalTag = ArithmeticalTagType.Additive, Feat = FeatFirstGuild.Entity, RoleType = RoleTypeGathering.Entity });

            var EquipmentSlotWeapon = EquipmentSlot.Add(new() { Name = "Weapon slot" });

            var UnitTypeHuman = UnitType.Add(new() { Name = "Human", EquipmentSlotList = new() { EquipmentSlotWeapon.Entity } });
            var UnitTypeElf = UnitType.Add(new() { Name = "Elf", EquipmentSlotList = new() { EquipmentSlotWeapon.Entity } });

            var SocketMasterwork = Socket.Add(new() { Name = "Masterwork", Description = "Improvements related to physical offense and defense." });
            var SocketRunemastery = Socket.Add(new() { Name = "Runemastery", Description = "Improvements related to magical offense and defense." });
            var SocketEnchantment = Socket.Add(new() { Name = "Enchantment", Description = "Improvements related to elemental offense and defense." });
            var SocketBlessing = Socket.Add(new() { Name = "Blessing", Description = "Improvements related to beneficial effects and recovering health." });
            var SocketCurse = Socket.Add(new() { Name = "Curse", Description = "Improvements related to detrimental effects and decaying health." });
            var SocketBeastcraft = Socket.Add(new() { Name = "Bloodpact", Description = "Improvements related to expending and stealing health." });

            var EquipmentIronSword = EquipmentItem.Add(new() { Name = "Iron Sword", Slot = EquipmentSlotWeapon.Entity, SocketList = new() { } });

            var EquipmentAnnealedIronSword = EquipmentItem.Add(new() { Name = "Annealed Iron Sword", Slot = EquipmentSlotWeapon.Entity, SocketList = new() { SocketMasterwork.Entity } });
            var EquipmentCursedIronSword = EquipmentItem.Add(new() { Name = "Cursed Iron Sword", Slot = EquipmentSlotWeapon.Entity, SocketList = new() { SocketCurse.Entity } });

            var EquipmentAnnealedCursedIronSword = EquipmentItem.Add(new() { Name = "Annealed Cursed Iron Sword", Slot = EquipmentSlotWeapon.Entity, SocketList = new() { SocketMasterwork.Entity, SocketCurse.Entity } });
            var EquipmentTemperedIronSword = EquipmentItem.Add(new() { Name = "Annealed Iron Sword", Slot = EquipmentSlotWeapon.Entity, SocketList = new() { SocketMasterwork.Entity, SocketMasterwork.Entity } });
            var EquipmentHexedIronSword = EquipmentItem.Add(new() { Name = "Annealed Iron Sword", Slot = EquipmentSlotWeapon.Entity, SocketList = new() { SocketCurse.Entity, SocketCurse.Entity } });

            var MaterialIronOre = MaterialItem.Add(new MaterialItemEntity() { Name = "Iron Ore", RoleType = RoleTypeGathering.Entity });
            var MaterialMoltenLodestone = MaterialItem.Add(new MaterialItemEntity() { Name = "Firestone Ore", RoleType = RoleTypeGathering.Entity });

            var MaterialDescecratedSoil = MaterialItem.Add(new MaterialItemEntity() { Name = "Descecrated Soil", RoleType = RoleTypeGathering.Entity });

            var RecipeIronSword = Recipe.Add(new() { Item = EquipmentIronSword.Entity });
            var RecipeItemIronSwordIronOre = RecipeItem.Add(new() { Count = 5, Item = MaterialIronOre.Entity, Recipe = RecipeIronSword.Entity });


            var RecipeAnnealedIronSword = Recipe.Add(new() { Item = EquipmentIronSword.Entity });
            var RecipeAnnealedIronSwordBase = RecipeItem.Add(new() { Count = 1, Item = MaterialIronOre.Entity, Recipe = RecipeIronSword.Entity });
            var RecipeAnnealedIronSwordRefinement = RecipeItem.Add(new() { Count = 5, Item = MaterialIronOre.Entity, Recipe = RecipeIronSword.Entity });

            ItemSocket.Add(new() { Item = EquipmentIronSword.Entity, Socket = SocketMasterwork.Entity });

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
