using Entity.Dtos;

namespace Data.Interfaces
{
    public interface IUserRepository : IGenericRepository<UserDto>
    {
        Task<UserDto> GetByUsernameAsync(string username);
        Task<UserDto> GetByEmailAsync(string email);
    }
}
