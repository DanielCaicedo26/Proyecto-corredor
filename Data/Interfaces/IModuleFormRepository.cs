using Entity.Dtos;

namespace Data.Interfaces
{
    public interface IModuleFormRepository : IGenericRepository<ModuleFormDto>
    {
        Task<List<ModuleFormDto>> GetModuleFormsByModuloAsync(int moduloId);
        Task<List<ModuleFormDto>> GetModuleFormsByFormaAsync(int formaId);
        Task<ModuleFormDto> GetByModuloAndFormaAsync(int moduloId, int formaId);
    }
}
