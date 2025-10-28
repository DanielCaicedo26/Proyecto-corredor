namespace Entity.Dtos.Auth
{
    /// <summary>
    /// Solicitud extendida de registro que incluye datos de la Persona
    /// </summary>
    public class RegisterRequestExtended
    {
        /// <summary>
        /// Nombre de usuario
        /// </summary>
        public string? Username { get; set; }

        /// <summary>
        /// Email del usuario
        /// </summary>
        public string? Email { get; set; }

        /// <summary>
        /// Contraseña
        /// </summary>
        public string? Password { get; set; }

        /// <summary>
        /// Confirmación de contraseña
        /// </summary>
        public string? ConfirmPassword { get; set; }

        // --- Datos de la Persona ---

        /// <summary>
        /// Nombre de la persona
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Apellido de la persona
        /// </summary>
        public string? LastName { get; set; }

        /// <summary>
        /// Teléfono de la persona
        /// </summary>
        public string? Phone { get; set; }

        /// <summary>
        /// Número de documento de la persona
        /// </summary>
        public string? DocumentNumber { get; set; }
    }
}
