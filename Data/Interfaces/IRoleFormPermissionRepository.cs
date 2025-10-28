using Entity.Dtos;

namespace Data.Interfaces
{
    public interface IRoleFormPermissionRepository : IGenericRepository<RoleFormPermissionDto>
    {
        Task<List<RoleFormPermissionDto>> GetPermissionsByRoleAndFormaAsync(int roleId, int formaId);
        Task<List<RoleFormPermissionDto>> GetPermissionsByRoleAsync(int roleId);
        Task<List<RoleFormPermissionDto>> GetPermissionsByFormaAsync(int formaId);
    }
}
