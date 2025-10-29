using Entity.Dtos;

namespace Bussines.Interfaces
{
    /// <summary>
    /// Interfaz para operaciones de Módulo-Forma (relación M:M).
    /// Hereda operaciones CRUD básicas: GetByIdAsync, GetAllAsync, CreateAsync, UpdateAsync, DeleteAsync
    /// </summary>
    public interface IModuleFormService : IGenericService<ModuleFormDto>
    {
        /// <summary>
        /// Obtiene todas las asociaciones módulo-forma para un módulo específico
        /// </summary>
        /// <param name="moduloId">ID del módulo (debe ser mayor a 0)</param>
        /// <returns>Lista de ModuleFormDto asociadas al módulo</returns>
        /// <exception cref="ArgumentException">Si moduloId es menor o igual a 0</exception>
        /// <exception cref="KeyNotFoundException">Si no se encuentra el módulo</exception>
        Task<List<ModuleFormDto>> GetModuleFormsByModuloAsync(int moduloId);

        /// <summary>
        /// Obtiene todas las asociaciones módulo-forma para una forma específica
        /// </summary>
        /// <param name="formaId">ID de la forma (debe ser mayor a 0)</param>
        /// <returns>Lista de ModuleFormDto asociadas a la forma</returns>
        /// <exception cref="ArgumentException">Si formaId es menor o igual a 0</exception>
        /// <exception cref="KeyNotFoundException">Si no se encuentra la forma</exception>
        Task<List<ModuleFormDto>> GetModuleFormsByFormaAsync(int formaId);

        /// <summary>
        /// Obtiene la asociación entre un módulo y una forma específica
        /// </summary>
        /// <param name="moduloId">ID del módulo (debe ser mayor a 0)</param>
        /// <param name="formaId">ID de la forma (debe ser mayor a 0)</param>
        /// <returns>ModuleFormDto si existe la asociación, null en caso contrario</returns>
        /// <exception cref="ArgumentException">Si moduloId o formaId son menores o iguales a 0</exception>
        Task<ModuleFormDto> GetByModuloAndFormaAsync(int moduloId, int formaId);
    }
}
