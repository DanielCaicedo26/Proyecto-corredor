using Entity.Dtos;

namespace Bussines.Interfaces
{
    /// <summary>
    /// Interfaz para operaciones de Persona.
    /// Hereda operaciones CRUD básicas: GetByIdAsync, GetAllAsync, CreateAsync, UpdateAsync, DeleteAsync
    /// </summary>
    public interface IPersonaService : IGenericService<PersonaDto>
    {
        /// <summary>
        /// Obtiene una persona por su número de documento
        /// </summary>
        /// <param name="documentNumber">Número de documento (no puede ser nulo ni vacío)</param>
        /// <returns>PersonaDto si existe, null en caso contrario</returns>
        /// <exception cref="ArgumentException">Si documentNumber es nulo o vacío</exception>
        Task<PersonaDto> GetByDocumentNumberAsync(string documentNumber);

        /// <summary>
        /// Obtiene todas las personas que tienen usuarios asociados
        /// </summary>
        /// <returns>Lista de PersonaDto con usuarios vinculados</returns>
        Task<List<PersonaDto>> GetPersonasWithUsersAsync();
    }
}
