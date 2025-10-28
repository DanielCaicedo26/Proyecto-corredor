using Entity.Dtos;

namespace Data.Interfaces
{
    public interface IPersonaRepository : IGenericRepository<PersonaDto>
    {
        Task<PersonaDto> GetByDocumentNumberAsync(string documentNumber);
        Task<List<PersonaDto>> GetPersonasWithUsersAsync();
    }
}
