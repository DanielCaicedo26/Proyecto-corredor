using Entity.Dtos;
using Data.Interfaces;
using Bussines.Interfaces;

namespace Bussines.Services
{
    public class ModuloService : GenericService<ModuloDto>, IModuloService
    {
        private readonly IModuloRepository _moduloRepository;

        public ModuloService(IModuloRepository moduloRepository) : base(moduloRepository)
        {
            _moduloRepository = moduloRepository;
        }

        public override async Task<ModuloDto> CreateAsync(ModuloDto dto)
        {
            if (dto == null)
                throw new ArgumentException("El módulo no puede ser nulo");

            ValidateData(dto);
            return await _moduloRepository.AddAsync(dto);
        }

        public override async Task<ModuloDto> UpdateAsync(ModuloDto dto)
        {
            if (dto == null)
                throw new ArgumentException("El módulo no puede ser nulo");

            if (dto.Id <= 0)
                throw new ArgumentException("ID debe ser mayor a 0");

            var existing = await _moduloRepository.GetByIdAsync(dto.Id);
            if (existing == null)
                throw new KeyNotFoundException("Módulo no encontrado");

            ValidateData(dto);

            return await _moduloRepository.UpdateAsync(dto);
        }

        public async Task<List<ModuloDto>> GetModulosByStatusAsync(string status)
        {
            return await _moduloRepository.GetModulosByStatusAsync(status);
        }

        public async Task<List<ModuloDto>> GetModulosWithFormasAsync()
        {
            return await _moduloRepository.GetModulosWithFormasAsync();
        }

        protected override void ValidateData(ModuloDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name))
                throw new ArgumentException("Name es requerido");

            if (dto.Name.Length < 2)
                throw new ArgumentException("Name debe tener mínimo 2 caracteres");
        }
    }
}
