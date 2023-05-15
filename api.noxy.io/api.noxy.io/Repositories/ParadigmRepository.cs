using Microsoft.EntityFrameworkCore;
using Database.Models.RPG;
using Database.Contexts;
using Database.Exceptions;
using Database.Enums;
using Database.Models.RPG.Abstract;

namespace Database.Repositories
{
    public interface IParadigmRepository
    {
        public Task<Paradigm> CreateParadigm(Guild guild, string name);
        public Task AssignUnit(Guild guild, Guid idParadigm, Guid idUnit);
        public Task AssignSkill(Guild guild, Guid idParadigmUnit, Guid idSkill);
    }

    public class ParadigmRepository : IParadigmRepository
    {
        private readonly RPGContext Context;

        public ParadigmRepository(RPGContext context)
        {
            Context = context;
        }

        public async Task<Paradigm> CreateParadigm(Guild guild, string name)
        {
            List<Paradigm> listParadigm = await Context.Paradigm.Where(x => x.Guild == guild).ToListAsync();
            List<ModifierFeat> listModifierTag = await Context.ModifierTag.Where(x => x.ModifierTag == ModifierTagType.Paradigm_Total && x.TemplateFeat.UnlockableFeatList.Any(y => y.Guild == guild)).ToListAsync();
            
            Modifier.AritmaticalSet setParadigm = Modifier.GetArithmaticalSet(listModifierTag);
            if (listParadigm.Count >= setParadigm.GetTotalInt())
            {
                throw new EntityOperationException<Paradigm>();
            }

            Paradigm entityParadigm = Context.Paradigm.Add(new Paradigm() { Name = name, Guild = guild }).Entity;
            await Context.SaveChangesAsync();
            return entityParadigm;
        }

        public async Task AssignUnit(Guild guild, Guid idParadigm, Guid idUnit)
        {
            Paradigm entityParadigm = await Context.Paradigm.FirstOrDefaultAsync(x => x.ID == idParadigm && x.Guild == guild)
                ?? throw new EntityNotFoundException<Paradigm>(new { Paradigm = idParadigm, Guild = guild.ID });

            Unit entityUnit = await Context.Unit.FirstOrDefaultAsync(x => x.ID == idUnit && x.Guild == guild)
                ?? throw new EntityNotFoundException<Unit>(new { Unit = idUnit, Guild = guild.ID });

            List<ParadigmUnit> listParadigmUnit = await Context.ParadigmUnit.Where(x => x.Paradigm == entityParadigm).ToListAsync();
            List<ModifierFeat> listModifierTag = await Context.ModifierTag.Where(x => x.ModifierTag == ModifierTagType.Paradigm_UnitCount && x.TemplateFeat.UnlockableFeatList.Any(y => y.Guild == guild)).ToListAsync();

            Modifier.AritmaticalSet setParadigmUnit = Modifier.GetArithmaticalSet(listModifierTag);
            if (listParadigmUnit.Count >= setParadigmUnit.GetTotalInt())
            {
                throw new EntityOperationException<Paradigm>();
            }

            Context.ParadigmUnit.Add(new ParadigmUnit() { Paradigm = entityParadigm, Unit = entityUnit });
            await Context.SaveChangesAsync();
        }

        public async Task AssignSkill(Guild guild, Guid idParadigmUnit, Guid idSkill)
        {
            ParadigmUnit entityParadigmUnit = await Context.ParadigmUnit.FirstOrDefaultAsync(x => x.ID == idParadigmUnit && x.Paradigm.Guild == guild)
                ?? throw new EntityNotFoundException<Paradigm>(new { ParadigmUnit = idParadigmUnit, Guild = guild.ID });

            Skill entitySkill = await Context.Skill.FirstOrDefaultAsync(x => x.ID == idSkill)
                ?? throw new EntityNotFoundException<Unit>(new { Skill = idSkill });

            List<ParadigmSkill> listParadigmSkill = await Context.ParadigmSkill.Where(x => x.ParadigmUnit == entityParadigmUnit).ToListAsync();
            List<ModifierFeat> listModifierTag = await Context.ModifierTag.Where(x => x.ModifierTag == ModifierTagType.Paradigm_UnitCount && x.TemplateFeat.UnlockableFeatList.Any(y => y.Guild == guild)).ToListAsync();

            Modifier.AritmaticalSet setParadigmSkill = Modifier.GetArithmaticalSet(listModifierTag);
            if (listParadigmSkill.Count >= setParadigmSkill.GetTotalInt())
            {
                throw new EntityOperationException<Paradigm>();
            }

            Context.ParadigmSkill.Add(new ParadigmSkill() { ParadigmUnit = entityParadigmUnit, Skill = entitySkill });
            await Context.SaveChangesAsync();
        }
    }
}
