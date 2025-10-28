using Entity.DBcontext;
using Entity.Dtos;
using Entity.Entities;
using Microsoft.EntityFrameworkCore;
using Data.Interfaces;
using AutoMapper;

namespace Data.Repositories
{
    public class PermissionRepository : GenericRepository<Permission, PermissionDto>, IPermissionRepository
    {
        public PermissionRepository(ApplicationDbContext context, IMapper mapper) : base(context, mapper)
        {
        }

        public async Task<PermissionDto> GetByNameAsync(string name)
        {
            var permission = await _dbSet
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Name == name);
            return _mapper.Map<PermissionDto>(permission);
        }

        public async Task<List<PermissionDto>> GetPermissionsByRoleAsync(int roleId)
        {
            var permissions = await _dbSet
                .AsNoTracking()
                .Where(p => p.RoleFormPermissions.Any(rfp => rfp.RoleId == roleId))
                .ToListAsync();
            return _mapper.Map<List<PermissionDto>>(permissions);
        }
    }
}
