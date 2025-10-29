using Entity.Dtos;

namespace Bussines.Interfaces
{
    /// <summary>
    /// Interfaz para operaciones de Rol.
    /// Hereda operaciones CRUD básicas: GetByIdAsync, GetAllAsync, CreateAsync, UpdateAsync, DeleteAsync
    /// </summary>
    public interface IRoleService : IGenericService<RoleDto>
    {
        /// <summary>
        /// Obtiene un rol por su nombre
        /// </summary>
        /// <param name="name">Nombre del rol (no puede ser nulo ni vacío)</param>
        /// <returns>RoleDto si existe, null en caso contrario</returns>
        /// <exception cref="ArgumentException">Si name es nulo o vacío</exception>
        Task<RoleDto> GetByNameAsync(string name);

        /// <summary>
        /// Obtiene todos los roles asignados a un usuario específico
        /// </summary>
        /// <param name="userId">ID del usuario (debe ser mayor a 0)</param>
        /// <returns>Lista de RoleDto asignados al usuario</returns>
        /// <exception cref="ArgumentException">Si userId es menor o igual a 0</exception>
        /// <exception cref="KeyNotFoundException">Si no se encuentra el usuario</exception>
        Task<List<RoleDto>> GetRolesByUserAsync(int userId);
    }
}
