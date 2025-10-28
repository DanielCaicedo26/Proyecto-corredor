namespace Modelo_de_security.Settings
{
    /// <summary>
    /// Configuración de JWT desde appsettings.json
    /// </summary>
    public class JwtSettings
    {
        public string SecretKey { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public int ExpirationMinutes { get; set; }
        public int RefreshTokenExpirationDays { get; set; }
    }
}
