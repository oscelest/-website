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
    }

    public class RPGRepository : IRPGRepository
    {
        private readonly RPGContext _db;
        private readonly IApplicationConfiguration _config;

        public RPGRepository(RPGContext db, IApplicationConfiguration config)
        {
            _db = db;
            _config = config;
        }

        public async Task<Guild> CreateGuild(User user, string name)
        {
            Guild entityGuild = (await _db.Guild.AddAsync(new Guild { Name = name, User = user })).Entity;

            List<TemplateFeat> listTemplateFeat = await _db.TemplateFeat.Where(x => x.TemplateRequirementList.Count == 0).ToListAsync();
            foreach (TemplateFeat entityTemplateFeat in listTemplateFeat)
            {
                await _db.UnlockableFeat.AddAsync(new()
                {
                    Guild = entityGuild,
                    TemplateFeat = entityTemplateFeat,
                    TimeAcquired = DateTime.UtcNow,
                });
            }

            _db.SaveChanges();

            return entityGuild;
        }

        public async Task<Guild?> LoadGuild(string name)
        {
            return await _db.Guild.FirstOrDefaultAsync(x => x.Name == name);
        }

        public async Task<Guild?> LoadGuild(User user)
        {
            return await _db.Guild.FirstOrDefaultAsync(x => x.User.ID == user.ID);
        }

        public async Task CleanUnitAvailableList(Guild guild)
        {
            _db.Unit.RemoveRange(await _db.Unit.Where(x => x.Guild.ID == guild.ID && x.TimeInitiated == null).ToListAsync());
            _db.SaveChanges();
        }

        public async Task<TemplateRecipe> GetRecipe(Guid recipeID)
        {
            return await _db.TemplateRecipe.FirstOrDefaultAsync(x => x.ID == recipeID)
                ?? throw new EntityNotFoundException<TemplateRecipe>(_db.TemplateRecipe, recipeID);
        }

        public async Task<bool> HasRecipe(Guid guild, Guid recipeID)
        {
            return (await _db.UnlockableRecipe.FirstOrDefaultAsync(x => x.Guild.ID == guild && x.TemplateRecipe.ID == recipeID)) != null;
        }

        public async Task CraftItem(Guild guild, TemplateRecipe recipe, List<Guid> listItem)
        {
            List<Item> items = await _db.Item.Where(x => listItem.Contains(x.ID)).ToListAsync();
            List<VolumeItemRecipe> listVolumeItemRecipe = await _db.VolumeItemRecipe.Where(x => x.TemplateRecipe.ID == recipe.ID).ToListAsync();
            List<VolumeItemRecipe> listInput = listVolumeItemRecipe.Where(x => x.Component).ToList();

            foreach (var entityVolumeItem in listInput)
            {
                var entityItem = items.FirstOrDefault(x => x.TemplateItem.ID == entityVolumeItem.TemplateItem.ID);
                if (entityItem == null) throw new Exception();



            }

            List<VolumeItemRecipe> listOutput = listVolumeItemRecipe.Where(x => !x.Component).ToList();



            foreach (VolumeItemRecipe item in listInput)
            {
                await _db.Item.FirstOrDefaultAsync(x => x.Guild.ID == x.ID);
            }
        }
    }
}
