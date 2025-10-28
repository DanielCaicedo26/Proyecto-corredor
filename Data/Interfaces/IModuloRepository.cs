using Entity.Dtos;

namespace Data.Interfaces
{
    public interface IModuloRepository : IGenericRepository<ModuloDto>
    {
        Task<List<ModuloDto>> GetModulosByStatusAsync(string status);
        Task<List<ModuloDto>> GetModulosWithFormasAsync();
    }
}
