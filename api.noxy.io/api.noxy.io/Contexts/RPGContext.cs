using Microsoft.EntityFrameworkCore;
using Database.Models.RPG;
using Database.Models.RPG.Junction;
using Database.Models.RPG.Abstract;

namespace Database.Contexts
{
    public class RPGContext : DbContext
    {
        public DbSet<TemplateItemGearUseTemplateSlot> TemplateItemGearUseTemplateSlot => Set<TemplateItemGearUseTemplateSlot>();
        public DbSet<TemplateItemSupportUseTemplateSlot> TemplateItemSupportUseTemplateSlot => Set<TemplateItemSupportUseTemplateSlot>();

        //public DbSet<Event> Event => Set<Event>();
        public DbSet<EventGuild> EventGuild => Set<EventGuild>();

        public DbSet<EquipmentSupport> EquipmentSupport => Set<EquipmentSupport>();
        public DbSet<EquipmentGear> EquipmentGear => Set<EquipmentGear>();

        public DbSet<Guild> Guild => Set<Guild>();

        public DbSet<Item> Item => Set<Item>();

        public DbSet<Mission> Mission => Set<Mission>();

        //public DbSet<Modifier> Modifier => Set<Modifier>();
        public DbSet<ModifierItem> ModifierItem => Set<ModifierItem>();
        public DbSet<ModifierFeat> ModifierTag => Set<ModifierFeat>();
        public DbSet<ModifierGuildMission> ModifierGuildMission => Set<ModifierGuildMission>();
        public DbSet<ModifierRole> ModifierGuildRole => Set<ModifierRole>();
        public DbSet<ModifierGuildUnit> ModifierGuildUnit => Set<ModifierGuildUnit>();

        public DbSet<Paradigm> Paradigm => Set<Paradigm>();
        public DbSet<ParadigmUnit> ParadigmUnit => Set<ParadigmUnit>();
        public DbSet<ParadigmSkill> ParadigmSkill => Set<ParadigmSkill>();

        public DbSet<Role> Role => Set<Role>();

        public DbSet<Skill> Skill => Set<Skill>();

        //public DbSet<Slot> Slot => Set<Slot>();
        public DbSet<SlotSupport> SlotSupport => Set<SlotSupport>();
        public DbSet<SlotGear> SlotGear => Set<SlotGear>();

        //public DbSet<Template> Template => Set<Template>();
        public DbSet<TemplateFeat> TemplateFeat => Set<TemplateFeat>();
        public DbSet<TemplateItem> TemplateItem => Set<TemplateItem>();
        public DbSet<TemplateItemSupport> TemplateItemSupport => Set<TemplateItemSupport>();
        public DbSet<TemplateItemGear> TemplateItemGear => Set<TemplateItemGear>();
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

            builder.Entity<Slot>().UseTpcMappingStrategy();
            builder.Entity<Equipment>().UseTpcMappingStrategy();
            builder.Entity<ModifierGuild>().UseTpcMappingStrategy();
            builder.Entity<TemplateItem>().UseTpcMappingStrategy();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySQL();
        }
    }
}
