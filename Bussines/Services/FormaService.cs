using Entity.Dtos;
using Data.Interfaces;
using Bussines.Interfaces;

namespace Bussines.Services
{
    public class FormaService : GenericService<FormaDto>, IFormaService
    {
        private readonly IFormaRepository _formaRepository;

        public FormaService(IFormaRepository formaRepository) : base(formaRepository)
        {
            _formaRepository = formaRepository;
        }

        public override async Task<FormaDto> CreateAsync(FormaDto dto)
        {
            if (dto == null)
                throw new ArgumentException("La forma no puede ser nula");

            ValidateData(dto);
            return await _formaRepository.AddAsync(dto);
        }

        public override async Task<FormaDto> UpdateAsync(FormaDto dto)
        {
            if (dto == null)
                throw new ArgumentException("La forma no puede ser nula");

            if (dto.Id <= 0)
                throw new ArgumentException("ID debe ser mayor a 0");

            var existing = await _formaRepository.GetByIdAsync(dto.Id);
            if (existing == null)
                throw new KeyNotFoundException("Forma no encontrada");

            ValidateData(dto);

            return await _formaRepository.UpdateAsync(dto);
        }

        public async Task<List<FormaDto>> GetFormasByModuloAsync(int moduloId)
        {
            if (moduloId <= 0)
                throw new ArgumentException("Modulo ID debe ser mayor a 0");

            return await _formaRepository.GetFormasByModuloAsync(moduloId);
        }

        public async Task<List<FormaDto>> GetFormasByStatusAsync(string status)
        {
            return await _formaRepository.GetFormasByStatusAsync(status);
        }

        protected override void ValidateData(FormaDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name))
                throw new ArgumentException("Name es requerido");

            if (dto.Name.Length < 2)
                throw new ArgumentException("Name debe tener mínimo 2 caracteres");
        }
    }
}
