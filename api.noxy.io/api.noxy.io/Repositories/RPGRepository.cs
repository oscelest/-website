using api.noxy.io.Context;
using api.noxy.io.Exceptions;
using api.noxy.io.Utility;
using api.noxy.io.Models.RPG;
using Microsoft.EntityFrameworkCore;
using Database.Models.RPG;
using Database.Models.RPG.Junction;

namespace api.noxy.io.Interface
{
    public interface IRPGRepository
    {
        public Task<Guild> CreateGuild(User user, string name);
        public Task<Guild?> LoadGuild(User user);
        public Task<Guild?> LoadGuild(string name);
        public Task CleanUnitAvailableList(Guild guild);
        public Task CraftItem(Guild guild, Guid recipeID, int count, List<Guid> listItemID);
        public Task EquipGear(Guild guild, Guid unitID, Guid itemID, List<Guid> listSlotID);
    }

    public class RPGRepository : IRPGRepository
    {
        private readonly RPGContext Context;
        private readonly IApplicationConfiguration Configuration;

        public RPGRepository(RPGContext context, IApplicationConfiguration configuration)
        {
            Context = context;
            Configuration = configuration;
        }

        public async Task<Guild> CreateGuild(User user, string name)
        {
            Guild entityGuild = (await Context.Guild.AddAsync(new Guild { Name = name, User = user })).Entity;

            List<TemplateFeat> listTemplateFeat = await Context.TemplateFeat.Where(x => x.TemplateRequirementList.Count == 0).ToListAsync();
            foreach (TemplateFeat entityTemplateFeat in listTemplateFeat)
            {
                await Context.UnlockableFeat.AddAsync(new()
                {
                    Guild = entityGuild,
                    TemplateFeat = entityTemplateFeat,
                    TimeAcquired = DateTime.UtcNow,
                });
            }

            Context.SaveChanges();

            return entityGuild;
        }

        public async Task<Guild?> LoadGuild(string name)
        {
            return await Context.Guild.FirstOrDefaultAsync(x => x.Name == name);
        }

        public async Task<Guild?> LoadGuild(User user)
        {
            return await Context.Guild.FirstOrDefaultAsync(x => x.User.ID == user.ID);
        }

        public async Task CleanUnitAvailableList(Guild guild)
        {
            Context.Unit.RemoveRange(await Context.Unit.Where(x => x.Guild.ID == guild.ID && x.TimeInitiated == null).ToListAsync());
            Context.SaveChanges();
        }

        public async Task<TemplateRecipe> GetRecipe(Guid recipeID)
        {
            return await Context.TemplateRecipe.FirstOrDefaultAsync(x => x.ID == recipeID)
                ?? throw new EntityNotFoundException<TemplateRecipe>(Context.TemplateRecipe, recipeID);
        }

        public async Task<bool> HasRecipe(Guid guild, Guid recipeID)
        {
            return (await Context.UnlockableRecipe.FirstOrDefaultAsync(x => x.Guild.ID == guild && x.TemplateRecipe.ID == recipeID)) != null;
        }

        public async Task CraftItem(Guild guild, Guid recipeID, int count, List<Guid> listItemID)
        {
            UnlockableRecipe entityUnlockableRecipe = await Context.UnlockableRecipe
                .FirstOrDefaultAsync(x => x.TemplateRecipe.ID == recipeID && x.Guild.ID == guild.ID)
                ?? throw new EntityNotFoundException<UnlockableRecipe>(Context.UnlockableRecipe, new { Recipe = recipeID, Guild = guild.ID });

            TemplateRecipe entityRecipe = entityUnlockableRecipe.TemplateRecipe;
            List<Item> listItem = await Context.Item.Where(x => x.Guild.ID == guild.ID && listItemID.Contains(x.ID)).ToListAsync();
            List<VolumeItemRecipe> listVolumeItem = await Context.VolumeItemRecipe.Where(x => x.TemplateRecipe.ID == entityRecipe.ID).ToListAsync();

            for (int i = 0; i < count; i++)
            {
                foreach (var entityVolumeItem in listVolumeItem.Where(x => x.Component))
                {
                    int requiredCount = entityVolumeItem.Count;
                    TemplateItem requiredItem = entityVolumeItem.TemplateItem;

                    Item entityItem = listItem.FirstOrDefault(x => x.TemplateItem.ID == requiredItem.ID)
                        ?? throw new EntityNotFoundException<Item>(Context.Item, new { TemplateItem = requiredItem.ID, Guild = guild.ID });

                    if (entityItem.Count < requiredCount)
                    {
                        throw new EntityConditionException<Item>(Context.Item, entityItem.ID);
                    }
                    entityItem.Count -= requiredCount;
                }

                foreach (VolumeItemRecipe entityVolumeItem in listVolumeItem.Where(x => !x.Component))
                {
                    Item? entityItem = listItem.FirstOrDefault(x => x.TemplateItem.ID == entityVolumeItem.TemplateItem.ID);
                    if (entityItem != null)
                    {
                        entityItem.Count += entityVolumeItem.Count;
                    }
                    else
                    {
                        Context.Item.Add(new() { Guild = guild, TemplateItem = entityVolumeItem.TemplateItem, Count = entityVolumeItem.Count });
                    }
                }
            }

            Context.SaveChanges();
        }

        public async Task EquipGear(Guild guild, Guid unitID, Guid itemID, List<Guid> listSlotID)
        {
            Item entityItem = await Context.Item
                .FirstOrDefaultAsync(x => x.ID == itemID && x.Guild.ID == guild.ID && x.Count > 0)
                ?? throw new EntityNotFoundException<Item>(Context.Item, new { Item = itemID, Guild = guild.ID });

            TemplateItemGear entityTemplateItemGear = entityItem.TemplateItem as TemplateItemGear
                ?? throw new EntityConditionException<Item>(Context.Item, itemID);

            List<TemplateItemGearWithTemplateSlot> listTemplateItemGearWithTemplateSlot = await Context.TemplateItemGearWithTemplateSlot
                .Where(x => x.TemplateItemGear.ID == entityTemplateItemGear.ID).ToListAsync();

            List<SlotGear> listSlotGear = await Context.SlotGear
                .Where(x => listSlotID.Contains(x.ID))
                .ToListAsync();

            List<SlotGear> listSlotGearToBeUsed = new();
            foreach (TemplateItemGearWithTemplateSlot entityTemplateItemGearWithTemplateSlot in listTemplateItemGearWithTemplateSlot)
            {
                SlotGear entitySlotGearToBeUsed = listSlotGear.FirstOrDefault(x => !listSlotGearToBeUsed.Contains(x) && x.TemplateSlot.ID == entityTemplateItemGearWithTemplateSlot.TemplateSlot.ID)
                    ?? throw new EntityNotFoundException<TemplateItemGearWithTemplateSlot>(Context.TemplateItemGearWithTemplateSlot, entityTemplateItemGearWithTemplateSlot.ID);
                listSlotGearToBeUsed.Add(entitySlotGearToBeUsed);
            }

            Unit entityUnit = await Context.Unit
                .FirstOrDefaultAsync(x => x.ID == unitID && x.Guild.ID == guild.ID)
                ?? throw new EntityNotFoundException<Unit>(Context.Unit, new { Unit = unitID, Guild = guild.ID });

            List<SlotGear> listSlotGearTemplateUnit = await Context.SlotGear.Where(x => x.TemplateUnitList.Any(x => x.ID == entityUnit.TemplateUnit.ID)).ToListAsync();
            IEnumerable<SlotGear> listSlotGearMissing = listSlotGearToBeUsed.Except(listSlotGearTemplateUnit);
            if (listSlotGearMissing.Any())
            {
                throw new EntityNotFoundException<SlotGear>(Context.SlotGear, listSlotGearMissing.Select(x => x.ID));
            }

            List<EquipmentGear> listEquipmentGear = await Context.EquipmentGear
                .Where(x => x.Unit!.ID == entityUnit.ID && x.SlotGearList.Any(x => listSlotGearToBeUsed.Contains(x)))
                .ToListAsync();

            foreach (EquipmentGear entityEquipmentGear in listEquipmentGear)
            {
                entityEquipmentGear.Unit = null;
            }

            entityItem.Count--;

            await Context.EquipmentGear.AddAsync(new EquipmentGear() { SlotGearList = listSlotGearToBeUsed, TemplateItemGear = entityTemplateItemGear, Unit = entityUnit });
            await Context.SaveChangesAsync();
        }

        public async Task EquipSupport(Guild entityGuild, Guid idEquipmentGear, Guid idItem)
        {

        }
    }
}
