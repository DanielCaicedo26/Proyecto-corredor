namespace Bussines.Interfaces
{
    /// <summary>
    /// Interfaz para configuración de JWT (JSON Web Token).
    /// Define los parámetros necesarios para generar y validar tokens JWT.
    /// </summary>
    public interface IJwtSettings
    {
        /// <summary>
        /// Clave secreta para firmar los tokens JWT.
        /// Debe tener al menos 32 caracteres para seguridad.
        /// </summary>
        string SecretKey { get; }

        /// <summary>
        /// Emisor del token (iss claim en JWT).
        /// Generalmente es el nombre de la aplicación.
        /// </summary>
        string Issuer { get; }

        /// <summary>
        /// Audiencia del token (aud claim en JWT).
        /// Generalmente es el nombre de la aplicación cliente.
        /// </summary>
        string Audience { get; }

        /// <summary>
        /// Tiempo de expiración del token en minutos.
        /// Valor típico: 60 (1 hora).
        /// </summary>
        int ExpirationMinutes { get; }

        /// <summary>
        /// Tiempo de expiración del refresh token en días.
        /// Valor típico: 7 (7 días).
        /// </summary>
        int RefreshTokenExpirationDays { get; }
    }
}
