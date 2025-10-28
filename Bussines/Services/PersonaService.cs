using Entity.Dtos;
using Data.Interfaces;
using Bussines.Interfaces;

namespace Bussines.Services
{
    public class PersonaService : GenericService<PersonaDto>, IPersonaService
    {
        private readonly IPersonaRepository _personaRepository;

        public PersonaService(IPersonaRepository personaRepository) : base(personaRepository)
        {
            _personaRepository = personaRepository;
        }

        public override async Task<PersonaDto> CreateAsync(PersonaDto dto)
        {
            if (dto == null)
                throw new ArgumentException("La persona no puede ser nula");

            ValidateData(dto);

            var existing = await _personaRepository.GetByDocumentNumberAsync(dto.DocumentNumber);
            if (existing != null)
                throw new InvalidOperationException("Este número de documento ya existe");

            return await _personaRepository.AddAsync(dto);
        }

        public override async Task<PersonaDto> UpdateAsync(PersonaDto dto)
        {
            if (dto == null)
                throw new ArgumentException("La persona no puede ser nula");

            if (dto.Id <= 0)
                throw new ArgumentException("ID debe ser mayor a 0");

            var existing = await _personaRepository.GetByIdAsync(dto.Id);
            if (existing == null)
                throw new KeyNotFoundException("Persona no encontrada");

            ValidateData(dto);

            return await _personaRepository.UpdateAsync(dto);
        }

        public async Task<PersonaDto> GetByDocumentNumberAsync(string documentNumber)
        {
            if (string.IsNullOrWhiteSpace(documentNumber))
                throw new ArgumentException("Número de documento es requerido");

            var persona = await _personaRepository.GetByDocumentNumberAsync(documentNumber);
            if (persona == null)
                throw new KeyNotFoundException("Persona no encontrada");

            return persona;
        }

        public async Task<List<PersonaDto>> GetPersonasWithUsersAsync()
        {
            return await _personaRepository.GetPersonasWithUsersAsync();
        }

        protected override void ValidateData(PersonaDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name))
                throw new ArgumentException("Name es requerido");

            if (string.IsNullOrWhiteSpace(dto.DocumentNumber))
                throw new ArgumentException("Número de documento es requerido");
        }
    }
}
