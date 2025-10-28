using Entity.Dtos;

namespace Bussines.Interfaces
{
    public interface IRoleService : IGenericService<RoleDto>
    {
        Task<RoleDto> GetByNameAsync(string name);
        Task<List<RoleDto>> GetRolesByUserAsync(int userId);
    }
}
