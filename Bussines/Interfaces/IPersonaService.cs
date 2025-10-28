using Entity.Dtos;

namespace Bussines.Interfaces
{
    public interface IPersonaService : IGenericService<PersonaDto>
    {
        Task<PersonaDto> GetByDocumentNumberAsync(string documentNumber);
        Task<List<PersonaDto>> GetPersonasWithUsersAsync();
    }
}
