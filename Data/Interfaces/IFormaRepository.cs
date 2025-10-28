using Entity.Dtos;

namespace Data.Interfaces
{
    public interface IFormaRepository : IGenericRepository<FormaDto>
    {
        Task<List<FormaDto>> GetFormasByModuloAsync(int moduloId);
        Task<List<FormaDto>> GetFormasByStatusAsync(string status);
    }
}
