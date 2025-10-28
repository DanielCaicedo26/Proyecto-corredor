using Entity.Dtos;
using Data.Interfaces;
using Bussines.Interfaces;

namespace Bussines.Services
{
    public class ModuleFormService : GenericService<ModuleFormDto>, IModuleFormService
    {
        private readonly IModuleFormRepository _moduleFormRepository;

        public ModuleFormService(IModuleFormRepository moduleFormRepository) : base(moduleFormRepository)
        {
            _moduleFormRepository = moduleFormRepository;
        }

        public async Task<ModuleFormDto> GetByIdAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException("ID debe ser mayor a 0");

            var moduleForm = await _moduleFormRepository.GetByIdAsync(id);
            if (moduleForm == null)
                throw new KeyNotFoundException($"ModuleForm con ID {id} no encontrado");

            return moduleForm;
        }

        public async Task<List<ModuleFormDto>> GetAllAsync()
        {
            return await _moduleFormRepository.GetAllAsync();
        }

        public async Task<ModuleFormDto> CreateAsync(ModuleFormDto dto)
        {
            ValidateModuleFormData(dto);
            return await _moduleFormRepository.AddAsync(dto);
        }

        public async Task<ModuleFormDto> UpdateAsync(ModuleFormDto dto)
        {
            var existing = await _moduleFormRepository.GetByIdAsync(dto.Id);
            if (existing == null)
                throw new KeyNotFoundException("ModuleForm no encontrado");

            ValidateModuleFormData(dto);
            return await _moduleFormRepository.UpdateAsync(dto);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var existing = await _moduleFormRepository.GetByIdAsync(id);
            if (existing == null)
                throw new KeyNotFoundException("ModuleForm no encontrado");

            return await _moduleFormRepository.DeleteAsync(id);
        }

        public async Task<List<ModuleFormDto>> GetModuleFormsByModuloAsync(int moduloId)
        {
            if (moduloId <= 0)
                throw new ArgumentException("Modulo ID debe ser mayor a 0");

            return await _moduleFormRepository.GetModuleFormsByModuloAsync(moduloId);
        }

        public async Task<List<ModuleFormDto>> GetModuleFormsByFormaAsync(int formaId)
        {
            if (formaId <= 0)
                throw new ArgumentException("Forma ID debe ser mayor a 0");

            return await _moduleFormRepository.GetModuleFormsByFormaAsync(formaId);
        }

        public async Task<ModuleFormDto> GetByModuloAndFormaAsync(int moduloId, int formaId)
        {
            if (moduloId <= 0 || formaId <= 0)
                throw new ArgumentException("IDs deben ser mayores a 0");

            return await _moduleFormRepository.GetByModuloAndFormaAsync(moduloId, formaId);
        }

        private void ValidateModuleFormData(ModuleFormDto dto)
        {
            if (dto.ModuloId <= 0)
                throw new ArgumentException("ModuloId es requerido");

            if (dto.FormaId <= 0)
                throw new ArgumentException("FormaId es requerido");
        }
    }
}
