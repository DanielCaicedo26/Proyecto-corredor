using Entity.Dtos;

namespace Bussines.Interfaces
{
    public interface IRoleFormPermissionService : IGenericService<RoleFormPermissionDto>
    {
        Task<List<RoleFormPermissionDto>> GetPermissionsByRoleAndFormaAsync(int roleId, int formaId);
        Task<List<RoleFormPermissionDto>> GetPermissionsByRoleAsync(int roleId);
        Task<List<RoleFormPermissionDto>> GetPermissionsByFormaAsync(int formaId);
    }
}
