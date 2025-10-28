using Entity.Dtos;

namespace Bussines.Interfaces
{
    public interface IUserService : IGenericService<UserDto>
    {
        Task<UserDto> GetByEmailAsync(string email);
        Task<UserDto> GetByUsernameAsync(string username);
        Task<List<UserDto>> GetUsersByRoleAsync(int roleId);
    }
}
