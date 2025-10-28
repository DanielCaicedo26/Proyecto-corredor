using Entity.Dtos;

namespace Bussines.Interfaces
{
    public interface IModuleFormService : IGenericService<ModuleFormDto>
    {
        Task<List<ModuleFormDto>> GetModuleFormsByModuloAsync(int moduloId);
        Task<List<ModuleFormDto>> GetModuleFormsByFormaAsync(int formaId);
        Task<ModuleFormDto> GetByModuloAndFormaAsync(int moduloId, int formaId);
    }
}
