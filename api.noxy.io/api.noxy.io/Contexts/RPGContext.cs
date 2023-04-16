using Microsoft.EntityFrameworkCore;
using api.noxy.io.Models.RPG;
using api.noxy.io.Models.RPG.Junction;

namespace api.noxy.io.Context
{
    public class RPGContext : DbContext
    {
        #region -- Junctions --

        public DbSet<EquipmentWithAugmentationInSlot> EquipmentAugmentationSlot => Set<EquipmentWithAugmentationInSlot>();
        public DbSet<UnitWithEquipmentInSlot> UnitEquipmentSlot => Set<UnitWithEquipmentInSlot>();

        #endregion -- Junctions --

        //public DbSet<Event> Event => Set<Event>();
        public DbSet<EventGuild> EventGuild => Set<EventGuild>();

        public DbSet<Guild> Guild => Set<Guild>();

        public DbSet<Item> Item => Set<Item>();
        public DbSet<ItemAugmentation> ItemAugmentation => Set<ItemAugmentation>();
        public DbSet<ItemEquipment> ItemEquipment => Set<ItemEquipment>();
        public DbSet<ItemMap> ItemMap => Set<ItemMap>();
        public DbSet<ItemMaterial> ItemMaterial => Set<ItemMaterial>();

        public DbSet<Mission> Mission => Set<Mission>();

        //public DbSet<Modifier> Modifier => Set<Modifier>();
        public DbSet<ModifierItem> ModifierItem => Set<ModifierItem>();
        public DbSet<ModifierGuild> ModifierGuild => Set<ModifierGuild>();
        public DbSet<ModifierGuildMission> ModifierGuildMission => Set<ModifierGuildMission>();
        public DbSet<ModifierGuildRole> ModifierGuildRole => Set<ModifierGuildRole>();
        public DbSet<ModifierGuildUnit> ModifierGuildUnit => Set<ModifierGuildUnit>();

        public DbSet<Role> Role => Set<Role>();

        //public DbSet<Slot> Slot => Set<Slot>();
        public DbSet<SlotAugmentation> SlotAugmentation => Set<SlotAugmentation>();
        public DbSet<SlotEquipment> SlotEquipment => Set<SlotEquipment>();

        //public DbSet<Template> Template => Set<Template>();
        public DbSet<TemplateFeat> TemplateFeat => Set<TemplateFeat>();
        public DbSet<TemplateItem> TemplateItem => Set<TemplateItem>();
        public DbSet<TemplateItemAugmentation> TemplateItemAugmentation => Set<TemplateItemAugmentation>();
        public DbSet<TemplateItemEquipment> TemplateItemEquipment => Set<TemplateItemEquipment>();
        public DbSet<TemplateItemMap> TemplateItemMap => Set<TemplateItemMap>();
        public DbSet<TemplateItemMaterial> TemplateItemMaterial => Set<TemplateItemMaterial>();
        public DbSet<TemplateMission> TemplateMission => Set<TemplateMission>();
        public DbSet<TemplateRecipe> TemplateRecipe => Set<TemplateRecipe>();
        public DbSet<TemplateRequirement> TemplateRequirement => Set<TemplateRequirement>();
        public DbSet<TemplateRole> TemplateRole => Set<TemplateRole>();
        public DbSet<TemplateSlot> TemplateSlot => Set<TemplateSlot>();
        public DbSet<TemplateUnit> TemplateUnit => Set<TemplateUnit>();

        public DbSet<Unit> Unit => Set<Unit>();

        //public DbSet<Unlockable> Unlockable => Set<Unlockable>();
        public DbSet<UnlockableFeat> UnlockableFeat => Set<UnlockableFeat>();
        public DbSet<UnlockableRecipe> UnlockableRecipe => Set<UnlockableRecipe>();
        public DbSet<UnlockableRole> UnlockableRole => Set<UnlockableRole>();

        public DbSet<User> User => Set<User>();

        //public DbSet<Volume> Volume => Set<Volume>();
        public DbSet<VolumeItemRecipe> VolumeItemRecipe => Set<VolumeItemRecipe>();

        public RPGContext() { }

        public RPGContext(DbContextOptions<RPGContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Item>().UseTpcMappingStrategy();
            builder.Entity<ModifierGuild>().UseTpcMappingStrategy();
            builder.Entity<TemplateItem>().UseTpcMappingStrategy();
        }

        public void Seed()
        {


            //    var RequirementRoleAdventuring = Requirement.Add(new() { });
            //    var RequirementRoleGathering = Requirement.Add(new() { });
            //    var RequirementRoleCrafting = Requirement.Add(new() { });

            //    var RoleTypeAdventuring = RoleType.Add(new() { Name = "Adventuring" });
            //    var RoleTypeGathering = RoleType.Add(new() { Name = "Gathering" });

            //    var FeatFirstGuild = Feat.Add(new() { Name = "First guild recruited" });
            //    var FeatFirstUnit = Feat.Add(new() { Name = "First unit recruited" });
            //    var FeatFirstMission = Feat.Add(new() { Name = "First mission completed" });
            //    var FeatFirstRole = Feat.Add(new() { Name = "First role unlocked" });
            //    var FeatFirstAdventure = Feat.Add(new() { Name = "First adventure completed" });

            //    Role.Add(new() { Name = "Cleric", RoleType = RoleTypeAdventuring.Entity });
            //    Role.Add(new() { Name = "Knight", RoleType = RoleTypeAdventuring.Entity });
            //    Role.Add(new() { Name = "Ranger", RoleType = RoleTypeAdventuring.Entity });
            //    Role.Add(new() { Name = "Paladin", RoleType = RoleTypeAdventuring.Entity, RequirementList = new() { RequirementRoleAdventuring.Entity } });
            //    Role.Add(new() { Name = "Dark Knight", RoleType = RoleTypeAdventuring.Entity, RequirementList = new() { RequirementRoleAdventuring.Entity } });

            //    Role.Add(new() { Name = "Miner", RoleType = RoleTypeGathering.Entity });
            //    Role.Add(new() { Name = "Furrier", RoleType = RoleTypeGathering.Entity });
            //    Role.Add(new() { Name = "Herbalist", RoleType = RoleTypeGathering.Entity });
            //    Role.Add(new() { Name = "Woodcutter", RoleType = RoleTypeGathering.Entity, RequirementList = new() { RequirementRoleGathering.Entity } });

            //    GuildRoleModifier.Add(new() { Tag = ModifierGuildRoleTagType.Count, Value = 1, ArithmeticalTag = ArithmeticalTagType.Additive, Feat = FeatFirstGuild.Entity });
            //    GuildRoleModifier.Add(new() { Tag = ModifierGuildRoleTagType.Count, Value = 1, ArithmeticalTag = ArithmeticalTagType.Additive, Feat = FeatFirstGuild.Entity, RoleType = RoleTypeAdventuring.Entity });
            //    GuildRoleModifier.Add(new() { Tag = ModifierGuildRoleTagType.Count, Value = 1, ArithmeticalTag = ArithmeticalTagType.Additive, Feat = FeatFirstGuild.Entity, RoleType = RoleTypeGathering.Entity });
            //    GuildRoleModifier.Add(new() { Tag = ModifierGuildRoleTagType.Experience, Value = 200, ArithmeticalTag = ArithmeticalTagType.Additive, Feat = FeatFirstGuild.Entity });
            //    GuildRoleModifier.Add(new() { Tag = ModifierGuildRoleTagType.Experience, Value = 300, ArithmeticalTag = ArithmeticalTagType.Additive, Feat = FeatFirstGuild.Entity, RoleType = RoleTypeAdventuring.Entity });
            //    GuildRoleModifier.Add(new() { Tag = ModifierGuildRoleTagType.Experience, Value = 100, ArithmeticalTag = ArithmeticalTagType.Additive, Feat = FeatFirstGuild.Entity, RoleType = RoleTypeGathering.Entity });

            //    var EquipmentSlotWeapon = EquipmentSlot.Add(new() { Name = "Weapon slot" });

            //    var UnitTypeHuman = UnitType.Add(new() { Name = "Human", EquipmentSlotList = new() { EquipmentSlotWeapon.Entity } });
            //    var UnitTypeElf = UnitType.Add(new() { Name = "Elf", EquipmentSlotList = new() { EquipmentSlotWeapon.Entity } });

            //    var SocketMasterwork = Socket.Add(new() { Name = "Masterwork", Description = "Improvements related to physical offense and defense." });
            //    var SocketRunemastery = Socket.Add(new() { Name = "Runemastery", Description = "Improvements related to magical offense and defense." });
            //    var SocketEnchantment = Socket.Add(new() { Name = "Enchantment", Description = "Improvements related to elemental offense and defense." });
            //    var SocketBlessing = Socket.Add(new() { Name = "Blessing", Description = "Improvements related to beneficial effects and recovering health." });
            //    var SocketCurse = Socket.Add(new() { Name = "Curse", Description = "Improvements related to detrimental effects and decaying health." });
            //    var SocketBeastcraft = Socket.Add(new() { Name = "Bloodpact", Description = "Improvements related to expending and stealing health." });

            //    var EquipmentIronSword = EquipmentItem.Add(new() { Name = "Iron Sword", Slot = EquipmentSlotWeapon.Entity, SocketList = new() { } });

            //    var EquipmentAnnealedIronSword = EquipmentItem.Add(new() { Name = "Annealed Iron Sword", Slot = EquipmentSlotWeapon.Entity, SocketList = new() { SocketMasterwork.Entity } });
            //    var EquipmentCursedIronSword = EquipmentItem.Add(new() { Name = "Cursed Iron Sword", Slot = EquipmentSlotWeapon.Entity, SocketList = new() { SocketCurse.Entity } });

            //    var EquipmentAnnealedCursedIronSword = EquipmentItem.Add(new() { Name = "Annealed Cursed Iron Sword", Slot = EquipmentSlotWeapon.Entity, SocketList = new() { SocketMasterwork.Entity, SocketCurse.Entity } });
            //    var EquipmentTemperedIronSword = EquipmentItem.Add(new() { Name = "Annealed Iron Sword", Slot = EquipmentSlotWeapon.Entity, SocketList = new() { SocketMasterwork.Entity, SocketMasterwork.Entity } });
            //    var EquipmentHexedIronSword = EquipmentItem.Add(new() { Name = "Annealed Iron Sword", Slot = EquipmentSlotWeapon.Entity, SocketList = new() { SocketCurse.Entity, SocketCurse.Entity } });

            //    var MaterialIronOre = MaterialItem.Add(new MaterialItemEntity() { Name = "Iron Ore", RoleType = RoleTypeGathering.Entity });
            //    var MaterialMoltenLodestone = MaterialItem.Add(new MaterialItemEntity() { Name = "Firestone Ore", RoleType = RoleTypeGathering.Entity });

            //    var MaterialDescecratedSoil = MaterialItem.Add(new MaterialItemEntity() { Name = "Descecrated Soil", RoleType = RoleTypeGathering.Entity });

            //    var RecipeIronSword = Recipe.Add(new() { Item = EquipmentIronSword.Entity });
            //    var RecipeItemIronSwordIronOre = RecipeItem.Add(new() { Count = 5, Item = MaterialIronOre.Entity, Recipe = RecipeIronSword.Entity });


            //    var RecipeAnnealedIronSword = Recipe.Add(new() { Item = EquipmentIronSword.Entity });
            //    var RecipeAnnealedIronSwordBase = RecipeItem.Add(new() { Count = 1, Item = MaterialIronOre.Entity, Recipe = RecipeIronSword.Entity });
            //    var RecipeAnnealedIronSwordRefinement = RecipeItem.Add(new() { Count = 5, Item = MaterialIronOre.Entity, Recipe = RecipeIronSword.Entity });

            //    ItemSocket.Add(new() { Item = EquipmentIronSword.Entity, Socket = SocketMasterwork.Entity });

            //    SaveChanges();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySQL();
        }

        //public override int SaveChanges()
        //{
        //    PerformEntityMaintenance();
        //    return base.SaveChanges();
        //}

        //public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        //{
        //    PerformEntityMaintenance();
        //    return await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken).ConfigureAwait(false);
        //}

        //private void PerformEntityMaintenance()
        //{
        //    IEnumerable<EntityEntry> entries = ChangeTracker.Entries();
        //    foreach (EntityEntry entry in entries)
        //    {
        //        if (entry.Entity is SingleEntity entity)
        //        {
        //            UpdateSingleEntityTimestamp(entity, entry.State);
        //        }
        //    }
        //}

        //private static void UpdateSingleEntityTimestamp(SingleEntity entity, EntityState state)
        //{
        //    if (state == EntityState.Added)
        //    {
        //        entity.TimeCreated = DateTime.UtcNow;
        //    }
        //    else if (state == EntityState.Modified)
        //    {
        //        entity.TimeUpdated = DateTime.UtcNow;
        //    }
        //}
    }
}
