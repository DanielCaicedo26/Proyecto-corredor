using Entity.Dtos;

namespace Bussines.Interfaces
{
    /// <summary>
    /// Interfaz para operaciones de Rol.
    /// Hereda operaciones CRUD básicas: GetByIdAsync, GetAllAsync, CreateAsync, UpdateAsync, DeleteAsync
    /// </summary>
    public interface IRoleService : IGenericService<RoleDto>
    {
    }
}
