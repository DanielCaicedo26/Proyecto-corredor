using Entity.Dtos;

namespace Data.Interfaces
{
    public interface IUserRoleRepository : IGenericRepository<UserRoleDto>
    {
        Task<List<UserRoleDto>> GetRolesByUserAsync(int userId);
        Task<List<UserRoleDto>> GetUsersByRoleAsync(int roleId);
        Task<bool> UserHasRoleAsync(int userId, int roleId);
        Task AddAsync(UserRoleDto userRole);
        Task<bool> DeleteAsync(int userId, int roleId);
    }
}
