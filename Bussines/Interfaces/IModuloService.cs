using Entity.Dtos;

namespace Bussines.Interfaces
{
    /// <summary>
    /// Interfaz para operaciones de Módulo.
    /// Hereda operaciones CRUD básicas: GetByIdAsync, GetAllAsync, CreateAsync, UpdateAsync, DeleteAsync
    /// </summary>
    public interface IModuloService : IGenericService<ModuloDto>
    {
    }
}
