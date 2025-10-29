using Entity.Dtos;

namespace Bussines.Interfaces
{
    /// <summary>
    /// Interfaz para operaciones de Rol-Forma-Permiso (relación M:M:M).
    /// Hereda operaciones CRUD básicas: GetByIdAsync, GetAllAsync, CreateAsync, UpdateAsync, DeleteAsync
    /// </summary>
    public interface IRoleFormPermissionService : IGenericService<RoleFormPermissionDto>
    {
    }
}
