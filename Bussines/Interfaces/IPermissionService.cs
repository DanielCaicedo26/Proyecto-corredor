using Entity.Dtos;

namespace Bussines.Interfaces
{
    public interface IPermissionService : IGenericService<PermissionDto>
    {
        Task<PermissionDto> GetByNameAsync(string name);
        Task<List<PermissionDto>> GetPermissionsByRoleAsync(int roleId);
    }
}
