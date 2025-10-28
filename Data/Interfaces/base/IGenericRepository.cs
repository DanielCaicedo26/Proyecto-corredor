using Entity.Dtos;

namespace Data.Interfaces
{
    public interface IGenericRepository<T> where T : BaseDto
    {
        Task<T> GetByIdAsync(int id);
        Task<List<T>> GetAllAsync();
        Task<T> AddAsync(T dto);
        Task<T> UpdateAsync(T dto);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}
