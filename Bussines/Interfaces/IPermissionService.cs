using Entity.Dtos;

namespace Bussines.Interfaces
{
    /// <summary>
    /// Interfaz para operaciones de Permiso.
    /// Hereda operaciones CRUD básicas: GetByIdAsync, GetAllAsync, CreateAsync, UpdateAsync, DeleteAsync
    /// </summary>
    public interface IPermissionService : IGenericService<PermissionDto>
    {
    }
}
