using Entity.Dtos;

namespace Data.Interfaces
{
    public interface IRoleRepository : IGenericRepository<RoleDto>
    {
        Task<RoleDto> GetByNameAsync(string name);
    }
}
