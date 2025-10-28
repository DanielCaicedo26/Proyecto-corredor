using Bussines.Interfaces;

namespace Modelo_de_security.Settings
{
    /// <summary>
    /// Configuración de JWT desde appsettings.json
    /// </summary>
    public class JwtSettings : IJwtSettings
    {
        public string SecretKey { get; set; } = string.Empty;
        public string Issuer { get; set; } = string.Empty;
        public string Audience { get; set; } = string.Empty;
        public int ExpirationMinutes { get; set; } = 60;
        public int RefreshTokenExpirationDays { get; set; } = 7;
    }
}
