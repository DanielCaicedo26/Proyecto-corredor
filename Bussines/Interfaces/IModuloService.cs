using Entity.Dtos;

namespace Bussines.Interfaces
{
    public interface IModuloService : IGenericService<ModuloDto>
    {
        Task<List<ModuloDto>> GetModulosByStatusAsync(string status);
        Task<List<ModuloDto>> GetModulosWithFormasAsync();
    }
}
