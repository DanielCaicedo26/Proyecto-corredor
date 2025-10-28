using Entity.Dtos;

namespace Bussines.Interfaces
{
    public interface IFormaService : IGenericService<FormaDto>
    {
        Task<List<FormaDto>> GetFormasByModuloAsync(int moduloId);
        Task<List<FormaDto>> GetFormasByStatusAsync(string status);
    }
}
