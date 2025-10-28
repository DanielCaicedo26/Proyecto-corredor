using Entity.DBcontext;
using Entity.Dtos;
using Entity.Entities;
using Microsoft.EntityFrameworkCore;
using Data.Interfaces;
using AutoMapper;

namespace Data.Repositories
{
    public class ModuloRepository : GenericRepository<Modulo, ModuloDto>, IModuloRepository
    {
        public ModuloRepository(ApplicationDbContext context, IMapper mapper) : base(context, mapper)
        {
        }

        public async Task<List<ModuloDto>> GetModulosByStatusAsync(string status)
        {
            var modulos = await _dbSet
                .AsNoTracking()
                .Where(m => m.Status == status)
                .ToListAsync();
            return _mapper.Map<List<ModuloDto>>(modulos);
        }

        public async Task<List<ModuloDto>> GetModulosWithFormasAsync()
        {
            var modulos = await _dbSet
                .AsNoTracking()
                .Where(m => m.ModuleForms.Any())
                .ToListAsync();
            return _mapper.Map<List<ModuloDto>>(modulos);
        }
    }
}
