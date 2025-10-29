using Entity.Dtos;

namespace Bussines.Interfaces
{
    /// <summary>
    /// Interfaz para operaciones de Usuario.
    /// Hereda operaciones CRUD básicas: GetByIdAsync, GetAllAsync, CreateAsync, UpdateAsync, DeleteAsync
    /// </summary>
    public interface IUserService : IGenericService<UserDto>
    {
        /// <summary>
        /// Obtiene un usuario por su correo electrónico
        /// </summary>
        /// <param name="email">Correo electrónico del usuario (no puede ser nulo ni vacío)</param>
        /// <returns>UserDto si existe, null en caso contrario</returns>
        /// <exception cref="ArgumentException">Si email es nulo o vacío</exception>
        Task<UserDto> GetByEmailAsync(string email);

        /// <summary>
        /// Obtiene un usuario por su nombre de usuario
        /// </summary>
        /// <param name="username">Nombre de usuario (no puede ser nulo ni vacío)</param>
        /// <returns>UserDto si existe, null en caso contrario</returns>
        /// <exception cref="ArgumentException">Si username es nulo o vacío</exception>
        Task<UserDto> GetByUsernameAsync(string username);

        /// <summary>
        /// Obtiene todos los usuarios que tienen asignado un rol específico
        /// </summary>
        /// <param name="roleId">ID del rol (debe ser mayor a 0)</param>
        /// <returns>Lista de UserDto asignados al rol</returns>
        /// <exception cref="ArgumentException">Si roleId es menor o igual a 0</exception>
        /// <exception cref="KeyNotFoundException">Si no se encuentra el rol</exception>
        Task<List<UserDto>> GetUsersByRoleAsync(int roleId);
    }
}
