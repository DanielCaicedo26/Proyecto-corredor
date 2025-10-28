using Entity.DBcontext;
using Entity.Dtos;
using Entity.Entities;
using Microsoft.EntityFrameworkCore;
using Data.Interfaces;
using AutoMapper;

namespace Data.Repositories
{
    public class ModuleFormRepository : GenericRepository<ModuleForm, ModuleFormDto>, IModuleFormRepository
    {
        public ModuleFormRepository(ApplicationDbContext context, IMapper mapper) : base(context, mapper)
        {
        }

        public async Task<List<ModuleFormDto>> GetModuleFormsByModuloAsync(int moduloId)
        {
            var moduleForms = await _dbSet
                .AsNoTracking()
                .Where(mf => mf.ModuloId == moduloId)
                .ToListAsync();
            return _mapper.Map<List<ModuleFormDto>>(moduleForms);
        }

        public async Task<List<ModuleFormDto>> GetModuleFormsByFormaAsync(int formaId)
        {
            var moduleForms = await _dbSet
                .AsNoTracking()
                .Where(mf => mf.FormaId == formaId)
                .ToListAsync();
            return _mapper.Map<List<ModuleFormDto>>(moduleForms);
        }

        public async Task<ModuleFormDto> GetByModuloAndFormaAsync(int moduloId, int formaId)
        {
            var moduleForm = await _dbSet
                .AsNoTracking()
                .FirstOrDefaultAsync(mf => mf.ModuloId == moduloId && mf.FormaId == formaId);
            return _mapper.Map<ModuleFormDto>(moduleForm);
        }
    }
}
