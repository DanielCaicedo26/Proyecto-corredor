using Entity.Dtos;

namespace Bussines.Interfaces
{
    public interface IGenericService<TDto> where TDto : BaseDto
    {
        Task<TDto> GetByIdAsync(int id);
        Task<List<TDto>> GetAllAsync();
        Task<TDto> CreateAsync(TDto dto);
        Task<TDto> UpdateAsync(TDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
