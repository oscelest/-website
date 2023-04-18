using api.noxy.io.Context;
using api.noxy.io.Exceptions;
using api.noxy.io.Utility;
using api.noxy.io.Models.RPG;
using Microsoft.EntityFrameworkCore;

namespace api.noxy.io.Interface
{
    public interface IRPGRepository
    {
        public Task<Guild> CreateGuild(User user, string name);
        public Task<Guild?> LoadGuild(User user);
        public Task<Guild?> LoadGuild(string name);
        public Task CleanUnitAvailableList(Guild guild);
        public Task CraftItem(Guild guild, Guid recipeID, List<Guid> listItemID);
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

        public async Task CraftItem(Guild guild, Guid recipeID, List<Guid> listItemID)
        {
            UnlockableRecipe entityUnlockableRecipe = await Context.UnlockableRecipe
                .FirstOrDefaultAsync(x => x.TemplateRecipe.ID == recipeID && x.Guild.ID == guild.ID)
                ?? throw new EntityNotFoundException<UnlockableRecipe>(Context.UnlockableRecipe, new { Recipe = recipeID, Guild = guild.ID });

            TemplateRecipe entityRecipe = entityUnlockableRecipe.TemplateRecipe;
            List<Item> listItem = await Context.Item.Where(x => x.Guild.ID == guild.ID && listItemID.Contains(x.ID)).ToListAsync();
            List<VolumeItemRecipe> listVolumeItem = await Context.VolumeItemRecipe.Where(x => x.TemplateRecipe.ID == entityRecipe.ID).ToListAsync();

            foreach (var entityVolumeItem in listVolumeItem.Where(x => x.Component))
            {
                int requiredCount = entityVolumeItem.Count;
                TemplateItem requiredItem = entityVolumeItem.TemplateItem;

                Item entityItem = listItem.FirstOrDefault(x => x.TemplateItem.ID == requiredItem.ID)
                    ?? throw new EntityNotFoundException<Item>(Context.Item, new { TemplateItem = requiredItem.ID, Guild = guild.ID });

                if (entityItem is ItemStackable entityItemStackable)
                {
                    if (entityItemStackable.Count < requiredCount)
                    {
                        throw new EntityConditionException<Item>(Context.Item, entityItemStackable.ID);
                    }
                    entityItemStackable.Count -= requiredCount;
                }
                else if (entityItem is ItemModifiable entityItemModifiable)
                {
                    Context.Item.Remove(entityItemModifiable);
                    listItem.Remove(entityItemModifiable);
                }
                else
                {
                    throw new Exception("Unknown item type.");
                }
            }

            foreach (VolumeItemRecipe entityVolumeItem in listVolumeItem.Where(x => !x.Component))
            {
                if (entityVolumeItem.TemplateItem is TemplateItemAugmentation entityTemplateItemAugmentation)
                {
                    Context.ItemAugmentation.Add(new() { Guild = guild, TemplateItem = entityTemplateItemAugmentation, Count = entityVolumeItem.Count });
                }
                else if (entityVolumeItem.TemplateItem is TemplateItemEquipment entityTemplateItemEquipment)
                {
                    Context.ItemEquipment.Add(new() { Guild = guild, TemplateItem = entityTemplateItemEquipment });
                }
                else
                {
                    throw new Exception();
                }
            }

            Context.SaveChanges();
        }
    }
}
