using Entity.Dtos;

namespace Bussines.Interfaces
{
    /// <summary>
    /// Interfaz para operaciones de Forma.
    /// Hereda operaciones CRUD básicas: GetByIdAsync, GetAllAsync, CreateAsync, UpdateAsync, DeleteAsync
    /// </summary>
    public interface IFormaService : IGenericService<FormaDto>
    {
        /// <summary>
        /// Obtiene todas las formas asociadas a un módulo específico
        /// </summary>
        /// <param name="moduloId">ID del módulo (debe ser mayor a 0)</param>
        /// <returns>Lista de FormaDto asociadas al módulo</returns>
        /// <exception cref="ArgumentException">Si moduloId es menor o igual a 0</exception>
        /// <exception cref="KeyNotFoundException">Si no se encuentra el módulo</exception>
        Task<List<FormaDto>> GetFormasByModuloAsync(int moduloId);

        /// <summary>
        /// Obtiene todas las formas con un estado específico
        /// </summary>
        /// <param name="status">Estado de la forma (ej: "Activo", "Inactivo")</param>
        /// <returns>Lista de FormaDto con el estado especificado</returns>
        /// <exception cref="ArgumentException">Si status es nulo o vacío</exception>
        Task<List<FormaDto>> GetFormasByStatusAsync(string status);
    }
}
