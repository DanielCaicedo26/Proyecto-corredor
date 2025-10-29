using Entity.Dtos.Auth;

namespace Bussines.Interfaces
{
    /// <summary>
    /// Interfaz para operaciones de autenticación y autorización
    /// </summary>
    public interface IAuthService
    {
        /// <summary>
        /// Realiza el login de un usuario con credenciales
        /// </summary>
        /// <param name="request">Datos de login (email/username y password)</param>
        /// <returns>AuthResponse con token JWT y datos del usuario</returns>
        /// <exception cref="ArgumentException">Si request es nulo o contiene datos inválidos</exception>
        /// <exception cref="UnauthorizedAccessException">Si las credenciales son incorrectas</exception>
        Task<AuthResponse> LoginAsync(LoginRequest request);

        /// <summary>
        /// Registra un nuevo usuario sin información de persona
        /// </summary>
        /// <param name="request">Datos de registro del usuario</param>
        /// <returns>AuthResponse con token JWT y datos del nuevo usuario</returns>
        /// <exception cref="ArgumentException">Si request es nulo o contiene datos inválidos</exception>
        /// <exception cref="InvalidOperationException">Si el email/usuario ya existe</exception>
        Task<AuthResponse> RegisterAsync(RegisterRequest request);

        /// <summary>
        /// Registra un nuevo usuario con información extendida de persona
        /// </summary>
        /// <param name="request">Datos de registro extendido (usuario + información personal)</param>
        /// <returns>AuthResponse con token JWT y datos del nuevo usuario y persona</returns>
        /// <exception cref="ArgumentException">Si request es nulo o contiene datos inválidos</exception>
        /// <exception cref="InvalidOperationException">Si el email/usuario o documento ya existe</exception>
        Task<AuthResponse> RegisterWithPersonaAsync(RegisterRequestExtended request);

        /// <summary>
        /// Genera un token JWT para un usuario específico
        /// </summary>
        /// <param name="userId">ID del usuario (debe ser mayor a 0)</param>
        /// <param name="username">Nombre de usuario del usuario</param>
        /// <returns>Token JWT codificado en string</returns>
        /// <exception cref="ArgumentException">Si userId es menor o igual a 0 o username es nulo/vacío</exception>
        string GenerateJwtToken(int userId, string username);
    }
}
