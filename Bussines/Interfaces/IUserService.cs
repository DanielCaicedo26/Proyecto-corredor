using Entity.Dtos;

namespace Bussines.Interfaces
{
    /// <summary>
    /// Interfaz para operaciones de Usuario.
    /// Hereda operaciones CRUD básicas: GetByIdAsync, GetAllAsync, CreateAsync, UpdateAsync, DeleteAsync
    /// </summary>
    public interface IUserService : IGenericService<UserDto>
    {
    }
}
