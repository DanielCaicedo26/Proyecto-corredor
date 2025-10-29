using Entity.Dtos;

namespace Bussines.Interfaces
{
    /// <summary>
    /// Interfaz para operaciones de Permiso.
    /// Hereda operaciones CRUD básicas: GetByIdAsync, GetAllAsync, CreateAsync, UpdateAsync, DeleteAsync
    /// </summary>
    public interface IPermissionService : IGenericService<PermissionDto>
    {
        /// <summary>
        /// Obtiene un permiso por su nombre
        /// </summary>
        /// <param name="name">Nombre del permiso (no puede ser nulo ni vacío)</param>
        /// <returns>PermissionDto si existe, null en caso contrario</returns>
        /// <exception cref="ArgumentException">Si name es nulo o vacío</exception>
        Task<PermissionDto> GetByNameAsync(string name);

        /// <summary>
        /// Obtiene todos los permisos asociados a un rol específico
        /// </summary>
        /// <param name="roleId">ID del rol (debe ser mayor a 0)</param>
        /// <returns>Lista de PermissionDto asociados al rol</returns>
        /// <exception cref="ArgumentException">Si roleId es menor o igual a 0</exception>
        /// <exception cref="KeyNotFoundException">Si no se encuentra el rol</exception>
        Task<List<PermissionDto>> GetPermissionsByRoleAsync(int roleId);
    }
}
