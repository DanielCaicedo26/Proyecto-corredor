using Entity.DBcontext;
using Entity.Dtos;
using Entity.Entities;
using Microsoft.EntityFrameworkCore;
using Data.Interfaces;
using AutoMapper;

namespace Data.Repositories
{
    public class RoleFormPermissionRepository : GenericRepository<RoleFormPermission, RoleFormPermissionDto>, IRoleFormPermissionRepository
    {
        public RoleFormPermissionRepository(ApplicationDbContext context, IMapper mapper) : base(context, mapper)
        {
        }

        public async Task<List<RoleFormPermissionDto>> GetPermissionsByRoleAndFormaAsync(int roleId, int formaId)
        {
            var permissions = await _dbSet
                .AsNoTracking()
                .Where(rfp => rfp.RoleId == roleId && rfp.FormaId == formaId)
                .ToListAsync();
            return _mapper.Map<List<RoleFormPermissionDto>>(permissions);
        }

        public async Task<List<RoleFormPermissionDto>> GetPermissionsByRoleAsync(int roleId)
        {
            var permissions = await _dbSet
                .AsNoTracking()
                .Where(rfp => rfp.RoleId == roleId)
                .ToListAsync();
            return _mapper.Map<List<RoleFormPermissionDto>>(permissions);
        }

        public async Task<List<RoleFormPermissionDto>> GetPermissionsByFormaAsync(int formaId)
        {
            var permissions = await _dbSet
                .AsNoTracking()
                .Where(rfp => rfp.FormaId == formaId)
                .ToListAsync();
            return _mapper.Map<List<RoleFormPermissionDto>>(permissions);
        }
    }
}
