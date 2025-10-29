using Entity.DBcontext;
using Entity.Dtos;
using Entity.Entities;
using Microsoft.EntityFrameworkCore;
using Data.Interfaces;
using AutoMapper;

namespace Data.Repositories
{
    public class RoleRepository : GenericRepository<Role, RoleDto>, IRoleRepository
    {
        public RoleRepository(ApplicationDbContext context, IMapper mapper) : base(context, mapper)
        {
        }

        public async Task<RoleDto> GetByNameAsync(string name)
        {
            var role = await _dbSet
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.Name == name);
            return _mapper.Map<RoleDto>(role);
        }
    }
}
