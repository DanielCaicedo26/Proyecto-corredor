using Entity.Dtos;

namespace Bussines.Interfaces
{
    /// <summary>
    /// Interfaz para operaciones de asignación de roles a usuarios (relación M:M).
    /// NO hereda de IGenericService porque su propósito es gestionar la relación entre usuarios y roles,
    /// no operaciones CRUD directas en la tabla de unión.
    /// </summary>
    public interface IUserRoleService
    {
        /// <summary>
        /// Asigna un rol a un usuario específico
        /// </summary>
        /// <param name="userId">ID del usuario (debe ser mayor a 0)</param>
        /// <param name="roleId">ID del rol (debe ser mayor a 0)</param>
        /// <returns>true si se asignó exitosamente, false si ya estaba asignado</returns>
        /// <exception cref="ArgumentException">Si userId o roleId son menores o iguales a 0</exception>
        /// <exception cref="KeyNotFoundException">Si no se encuentran el usuario o el rol</exception>
        Task<bool> AssignRoleToUserAsync(int userId, int roleId);

        /// <summary>
        /// Remueve un rol de un usuario específico
        /// </summary>
        /// <param name="userId">ID del usuario (debe ser mayor a 0)</param>
        /// <param name="roleId">ID del rol (debe ser mayor a 0)</param>
        /// <returns>true si se removió exitosamente, false si no estaba asignado</returns>
        /// <exception cref="ArgumentException">Si userId o roleId son menores o iguales a 0</exception>
        /// <exception cref="KeyNotFoundException">Si no se encuentran el usuario o el rol</exception>
        Task<bool> RemoveRoleFromUserAsync(int userId, int roleId);

        /// <summary>
        /// Obtiene todos los roles asignados a un usuario específico
        /// </summary>
        /// <param name="userId">ID del usuario (debe ser mayor a 0)</param>
        /// <returns>Lista de RoleDto asignados al usuario</returns>
        /// <exception cref="ArgumentException">Si userId es menor o igual a 0</exception>
        /// <exception cref="KeyNotFoundException">Si no se encuentra el usuario</exception>
        Task<List<RoleDto>> GetUserRolesAsync(int userId);

        /// <summary>
        /// Verifica si un usuario tiene un rol específico asignado
        /// </summary>
        /// <param name="userId">ID del usuario (debe ser mayor a 0)</param>
        /// <param name="roleId">ID del rol (debe ser mayor a 0)</param>
        /// <returns>true si el usuario tiene el rol, false en caso contrario</returns>
        /// <exception cref="ArgumentException">Si userId o roleId son menores o iguales a 0</exception>
        /// <exception cref="KeyNotFoundException">Si no se encuentran el usuario o el rol</exception>
        Task<bool> UserHasRoleAsync(int userId, int roleId);
    }
}
