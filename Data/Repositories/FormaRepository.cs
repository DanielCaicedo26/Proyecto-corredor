using Entity.DBcontext;
using Entity.Dtos;
using Entity.Entities;
using Microsoft.EntityFrameworkCore;
using Data.Interfaces;
using AutoMapper;

namespace Data.Repositories
{
    public class FormaRepository : GenericRepository<Forma, FormaDto>, IFormaRepository
    {
        public FormaRepository(ApplicationDbContext context, IMapper mapper) : base(context, mapper)
        {
        }

        public async Task<List<FormaDto>> GetFormasByModuloAsync(int moduloId)
        {
            var formas = await _dbSet
                .AsNoTracking()
                .Where(f => f.ModuleForms.Any(mf => mf.ModuloId == moduloId))
                .ToListAsync();
            return _mapper.Map<List<FormaDto>>(formas);
        }

        public async Task<List<FormaDto>> GetFormasByStatusAsync(string status)
        {
            var formas = await _dbSet
                .AsNoTracking()
                .Where(f => f.Status == status)
                .ToListAsync();
            return _mapper.Map<List<FormaDto>>(formas);
        }
    }
}
