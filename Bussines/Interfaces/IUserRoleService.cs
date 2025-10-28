using Entity.Dtos;

namespace Bussines.Interfaces
{
    public interface IUserRoleService
    {
        Task<bool> AssignRoleToUserAsync(int userId, int roleId);
        Task<bool> RemoveRoleFromUserAsync(int userId, int roleId);
        Task<List<RoleDto>> GetUserRolesAsync(int userId);
        Task<bool> UserHasRoleAsync(int userId, int roleId);
    }
}
