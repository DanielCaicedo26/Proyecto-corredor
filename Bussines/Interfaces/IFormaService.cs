using Entity.Dtos;

namespace Bussines.Interfaces
{
    /// <summary>
    /// Interfaz para operaciones de Forma.
    /// Hereda operaciones CRUD básicas: GetByIdAsync, GetAllAsync, CreateAsync, UpdateAsync, DeleteAsync
    /// </summary>
    public interface IFormaService : IGenericService<FormaDto>
    {
    }
}
