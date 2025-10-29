using Entity.Dtos;

namespace Bussines.Interfaces
{
    /// <summary>
    /// Interfaz para operaciones de Rol-Forma-Permiso (relación M:M:M).
    /// Hereda operaciones CRUD básicas: GetByIdAsync, GetAllAsync, CreateAsync, UpdateAsync, DeleteAsync
    /// </summary>
    public interface IRoleFormPermissionService : IGenericService<RoleFormPermissionDto>
    {
        /// <summary>
        /// Obtiene todos los permisos de forma para un rol y forma específica
        /// </summary>
        /// <param name="roleId">ID del rol (debe ser mayor a 0)</param>
        /// <param name="formaId">ID de la forma (debe ser mayor a 0)</param>
        /// <returns>Lista de RoleFormPermissionDto para la combinación rol-forma</returns>
        /// <exception cref="ArgumentException">Si roleId o formaId son menores o iguales a 0</exception>
        /// <exception cref="KeyNotFoundException">Si no se encuentran el rol o la forma</exception>
        Task<List<RoleFormPermissionDto>> GetPermissionsByRoleAndFormaAsync(int roleId, int formaId);

        /// <summary>
        /// Obtiene todos los permisos de forma asociados a un rol específico
        /// </summary>
        /// <param name="roleId">ID del rol (debe ser mayor a 0)</param>
        /// <returns>Lista de RoleFormPermissionDto asociados al rol</returns>
        /// <exception cref="ArgumentException">Si roleId es menor o igual a 0</exception>
        /// <exception cref="KeyNotFoundException">Si no se encuentra el rol</exception>
        Task<List<RoleFormPermissionDto>> GetPermissionsByRoleAsync(int roleId);

        /// <summary>
        /// Obtiene todos los permisos de forma asociados a una forma específica
        /// </summary>
        /// <param name="formaId">ID de la forma (debe ser mayor a 0)</param>
        /// <returns>Lista de RoleFormPermissionDto asociados a la forma</returns>
        /// <exception cref="ArgumentException">Si formaId es menor o igual a 0</exception>
        /// <exception cref="KeyNotFoundException">Si no se encuentra la forma</exception>
        Task<List<RoleFormPermissionDto>> GetPermissionsByFormaAsync(int formaId);
    }
}
