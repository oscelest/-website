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
        public Task<EquipmentGear> EquipGear(Guild guild, Guid unitID, Guid itemID, List<Guid> listSlotID);
        public Task EquipSupport(Guild guild, Guid equipmentID, Guid itemID, List<Guid> listSlotID);
        public Task UnequipGear(Guild guild, Guid equipmentID);
        public Task ClearEquipment(Guild guild);
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
                ?? throw new EntityNotFoundException<TemplateRecipe>(recipeID);
        }

        public async Task<bool> HasRecipe(Guid guild, Guid recipeID)
        {
            return (await Context.UnlockableRecipe.FirstOrDefaultAsync(x => x.Guild.ID == guild && x.TemplateRecipe.ID == recipeID)) != null;
        }

        public async Task CraftItem(Guild guild, Guid recipeID, int count, List<Guid> listItemID)
        {
            UnlockableRecipe entityUnlockableRecipe = await Context.UnlockableRecipe
                .FirstOrDefaultAsync(x => x.TemplateRecipe.ID == recipeID && x.Guild.ID == guild.ID)
                ?? throw new EntityNotFoundException<UnlockableRecipe>(new { Recipe = recipeID, Guild = guild.ID });

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
                        ?? throw new EntityNotFoundException<Item>(new { TemplateItem = requiredItem.ID, Guild = guild.ID });

                    if (entityItem.Count < requiredCount)
                    {
                        throw new EntityConditionException<Item>(entityItem.ID);
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

        public async Task<EquipmentGear> EquipGear(Guild guild, Guid unitID, Guid itemID, List<Guid> listSlotGearID)
        {
            Unit entityUnit = await LoadUnit(guild.ID, unitID);
            Item entityItem = await LoadItem(guild.ID, itemID);
            TemplateItemGear entityTemplateItemGear = GetItemTemplate<TemplateItemGear>(entityItem);

            List<SlotGear> listSlotGear = await GetTemplateItemSlotGearCombination(entityTemplateItemGear.ID, entityUnit.TemplateUnit.ID, listSlotGearID);
            if (!listSlotGear.Any())
            {
                throw new EntityOperationException<TemplateItemGear>();
            }

            if (listSlotGearID.Count < listSlotGear.Count)
            {
                throw new EntityNotFoundException<SlotGear>(listSlotGearID.Where(x => !listSlotGear.Any(y => y.ID == x)));
            }

            List<EquipmentGear> listEquipmentGear = await Context.EquipmentGear.Where(x => x.Unit!.ID == entityUnit.ID && x.SlotGearList.Any(x => listSlotGear.Contains(x))).ToListAsync();
            foreach (EquipmentGear itemEquipmentGear in listEquipmentGear)
            {
                itemEquipmentGear.Unit = null;
            }

            entityItem.Count--;

            EquipmentGear entityEquipmentGear = new() { SlotGearList = listSlotGear, TemplateItemGear = entityTemplateItemGear, Unit = entityUnit, Guild = guild };
            await Context.EquipmentGear.AddAsync(entityEquipmentGear);
            await Context.SaveChangesAsync();
            return entityEquipmentGear;
        }

        public async Task EquipSupport(Guild guild, Guid equipmentID, Guid itemID, List<Guid> listSlotSupportID)
        {
            EquipmentGear entityEquipmentGear = await LoadEquipmentGear(guild.ID, equipmentID);
            Item entityItem = await LoadItem(guild.ID, itemID);
            TemplateItemSupport entityTemplateItemSupport = GetItemTemplate<TemplateItemSupport>(entityItem);

            List<SlotSupport> listSlotSupport = await GetTemplateItemSlotSupportCombination(entityTemplateItemSupport.ID, entityEquipmentGear.TemplateItemGear.ID, listSlotSupportID);
            if (!listSlotSupport.Any())
            {
                throw new EntityOperationException<TemplateItemSupport>();
            }
            if (listSlotSupportID.Count < listSlotSupport.Count)
            {
                throw new EntityNotFoundException<SlotGear>(listSlotSupportID.Where(x => !listSlotSupport.Any(y => y.ID == x)));
            }

            List<EquipmentSupport> listEquipmentSupport = await Context.EquipmentSupport.Where(x => x.EquipmentGear!.ID == entityEquipmentGear.ID && x.SlotSupportList.Any(x => listSlotSupport.Contains(x))).ToListAsync();
            foreach (EquipmentSupport entityEquipmentSupport in listEquipmentSupport)
            {
                var item = await Context.Item.FirstOrDefaultAsync(x => x.TemplateItem.ID == entityEquipmentSupport.TemplateItemSupport.ID);
                if (item == null)
                {
                    await Context.Item.AddAsync(new() { TemplateItem = entityEquipmentSupport.TemplateItemSupport, Guild = guild, Count = 1 });
                }
                else
                {
                    item.Count++;
                }
            }

            entityItem.Count--;

            await Context.EquipmentSupport.AddAsync(new EquipmentSupport() { Guild = guild, EquipmentGear = entityEquipmentGear, TemplateItemSupport = entityTemplateItemSupport });
            await Context.SaveChangesAsync();
        }

        public async Task UnequipGear(Guild guild, Guid equipmentID)
        {
            EquipmentGear entityEquipmentGear = await Context.EquipmentGear.FirstOrDefaultAsync(x => x.ID == equipmentID && x.Guild.ID == guild.ID)
                ?? throw new EntityNotFoundException<EquipmentGear>(equipmentID);

            entityEquipmentGear.Unit = null;

            await Context.SaveChangesAsync();
        }

        public async Task ClearEquipment(Guild guild)
        {
            List<EquipmentGear> listEquipmentGear = await Context.EquipmentGear.Where(x => x.Guild.ID == guild.ID && x.Unit == null).ToListAsync();
            foreach (EquipmentGear equipment in listEquipmentGear)
            {
                Item? Item = await Context.Item.FirstOrDefaultAsync(x => x.TemplateItem.ID == equipment.TemplateItemGear.ID && x.Guild.ID == guild.ID);
                if (Item == null)
                {
                    await Context.Item.AddAsync(new Item() { Count = 1, Guild = guild, TemplateItem = equipment.TemplateItemGear });
                }
                else
                {
                    Item.Count++;
                }
                Context.EquipmentGear.Remove(equipment);
            }

            await Context.SaveChangesAsync();
        }

        private async Task<Unit> LoadUnit(Guid guildID, Guid unitID)
        {
            return await Context.Unit.FirstOrDefaultAsync(x => x.ID == unitID && x.Guild.ID == guildID)
                ?? throw new EntityNotFoundException<Unit>(new { Unit = unitID, Guild = guildID });
        }

        private async Task<EquipmentGear> LoadEquipmentGear(Guid guildID, Guid equipmentID)
        {
            return await Context.EquipmentGear.FirstOrDefaultAsync(x => x.ID == equipmentID && x.Guild.ID == guildID)
                ?? throw new EntityNotFoundException<EquipmentGear>(new { EquipmentGear = equipmentID, Guild = guildID });
        }

        private async Task<Item> LoadItem(Guid guildID, Guid itemID, int count = 0)
        {
            return await Context.Item.FirstOrDefaultAsync(x => x.ID == itemID && x.Guild.ID == guildID && x.Count > count)
                ?? throw new EntityNotFoundException<Item>(new { Item = itemID, Guild = guildID });
        }

        private T GetItemTemplate<T>(Item item) where T : TemplateItem
        {
            return item.TemplateItem as T ?? throw new EntityConditionException<Item>(item.ID);
        }

        private async Task<List<SlotGear>> GetTemplateItemSlotGearCombination(Guid templateItemGear, Guid templateUnit, IEnumerable<Guid> listSlotGearID)
        {
            List<SlotGear> listSlotGear = await Context.SlotGear.Where(x => x.TemplateUnitList.Any(x => x.ID == templateUnit) && listSlotGearID.Contains(x.ID)).ToListAsync();
            List<TemplateItemGearUseTemplateSlot> listA = await Context.TemplateItemGearUseTemplateSlot.Where(x => x.TemplateItemGear.ID == templateItemGear).ToListAsync();

            List<Guid> listMissing = new();
            List<SlotGear> listAvailable = new();

            foreach (TemplateItemGearUseTemplateSlot itemA in listA)
            {
                SlotGear? itemB = listSlotGear.FirstOrDefault(x => x.TemplateSlot.ID == itemA.TemplateSlot.ID);
                if (itemB != null)
                {
                    listAvailable.Add(itemB);
                    listSlotGear.Remove(itemB);
                }
                else
                {
                    listMissing.Add(itemA.ID);
                }
            }

            if (listMissing.Any())
            {
                throw new EntityNotFoundException<TemplateItemGearUseTemplateSlot>(listMissing);
            }

            return listAvailable;
        }

        private async Task<List<SlotSupport>> GetTemplateItemSlotSupportCombination(Guid templateItemSupport, Guid templateItemGear, IEnumerable<Guid> listSlotSupportID)
        {
            List<SlotSupport> listSlotSupport = await Context.SlotSupport.Where(x => x.TemplateItemGearList.Any(x => x.ID == templateItemGear) && listSlotSupportID.Contains(x.ID)).ToListAsync();
            List<TemplateItemSupportUseTemplateSlot> listA = await Context.TemplateItemSupportUseTemplateSlot.Where(x => x.TemplateItemSupport.ID == templateItemSupport).ToListAsync();

            List<Guid> listMissing = new();
            List<SlotSupport> listAvailable = new();

            foreach (TemplateItemSupportUseTemplateSlot itemA in listA)
            {
                SlotSupport? itemB = listSlotSupport.FirstOrDefault(x => x.TemplateSlot.ID == itemA.TemplateSlot.ID);
                if (itemB != null)
                {
                    listAvailable.Add(itemB);
                    listSlotSupport.Remove(itemB);
                }
                else
                {
                    listMissing.Add(itemA.ID);
                }
            }

            if (listMissing.Any())
            {
                throw new EntityNotFoundException<TemplateItemSupportUseTemplateSlot>(listMissing);
            }

            return listAvailable;
        }
    }
}
