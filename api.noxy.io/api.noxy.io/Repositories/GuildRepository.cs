using Database.Models.RPG;
using Database.Contexts;

namespace Database.Repositories
{
    public interface IGuildRepository
    {
        public Task<Guild> CreateGuild(User user, string name);
    }

    public class GuildRepository : IGuildRepository
    {
        private readonly RPGContext Context;

        public GuildRepository(RPGContext context)
        {
            Context = context;
        }

        public async Task<Guild> CreateGuild(User user, string name)
        {
            Guild entityGuild = Context.Guild.Add(new() { Name = name, User = user }).Entity;

            List<TemplateFeat> listTemplateFeat = Context.TemplateFeat.Where(x => x.TemplateRequirementList.Count() == 0).ToList();
            foreach (TemplateFeat entityTemplateFeat in listTemplateFeat)
            {
                Context.UnlockableFeat.Add(new() { Guild = entityGuild, TemplateFeat = entityTemplateFeat });
            }

            Context.Paradigm.Add(new() { Guild = entityGuild, Name = "Paradigm #1", });

            await Context.SaveChangesAsync();
            return entityGuild;
        }
    }
}
