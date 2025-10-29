using Entity.Dtos;

namespace Bussines.Interfaces
{
    /// <summary>
    /// Interfaz para operaciones de Módulo.
    /// Hereda operaciones CRUD básicas: GetByIdAsync, GetAllAsync, CreateAsync, UpdateAsync, DeleteAsync
    /// </summary>
    public interface IModuloService : IGenericService<ModuloDto>
    {
        /// <summary>
        /// Obtiene todos los módulos con un estado específico
        /// </summary>
        /// <param name="status">Estado del módulo (ej: "Activo", "Inactivo")</param>
        /// <returns>Lista de ModuloDto con el estado especificado</returns>
        /// <exception cref="ArgumentException">Si status es nulo o vacío</exception>
        Task<List<ModuloDto>> GetModulosByStatusAsync(string status);

        /// <summary>
        /// Obtiene todos los módulos con sus formas asociadas
        /// </summary>
        /// <returns>Lista de ModuloDto incluyendo sus formas relacionadas</returns>
        Task<List<ModuloDto>> GetModulosWithFormasAsync();
    }
}
