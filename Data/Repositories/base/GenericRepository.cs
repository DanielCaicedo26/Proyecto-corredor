using Entity.DBcontext;
using Entity.Dtos;
using Microsoft.EntityFrameworkCore;
using Data.Interfaces;
using AutoMapper;

namespace Data.Repositories
{
    public class GenericRepository<TEntity, TDto> : IGenericRepository<TDto>
        where TEntity : class
        where TDto : BaseDto
    {
        protected readonly ApplicationDbContext _context;
        protected readonly DbSet<TEntity> _dbSet;
        protected readonly IMapper _mapper;

        public GenericRepository(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
            _mapper = mapper;
        }

        public virtual async Task<TDto> GetByIdAsync(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            return _mapper.Map<TDto>(entity);
        }

        public virtual async Task<List<TDto>> GetAllAsync()
        {
            var entities = await _dbSet.ToListAsync();
            return _mapper.Map<List<TDto>>(entities);
        }

        public virtual async Task<TDto> AddAsync(TDto dto)
        {
            var entity = _mapper.Map<TEntity>(dto);
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
            return _mapper.Map<TDto>(entity);
        }

        public virtual async Task<TDto> UpdateAsync(TDto dto)
        {
            var entity = _mapper.Map<TEntity>(dto);
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
            return _mapper.Map<TDto>(entity);
        }

        public virtual async Task<bool> DeleteAsync(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity != null)
            {
                _dbSet.Remove(entity);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public virtual async Task<bool> ExistsAsync(int id)
        {
            return await _dbSet.FindAsync(id) != null;
        }
    }
}
