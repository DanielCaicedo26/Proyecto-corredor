using Entity.Dtos;

namespace Data.Interfaces
{
    public interface IPermissionRepository : IGenericRepository<PermissionDto>
    {
        Task<PermissionDto> GetByNameAsync(string name);
        Task<List<PermissionDto>> GetPermissionsByRoleAsync(int roleId);
    }
}
