using System.ComponentModel.DataAnnotations;

namespace Entity.Entities
{
    public class User : BaseEntity
    {
        public int? PersonaId { get; set; }

        [Required(ErrorMessage = "El nombre de usuario es requerido")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "El nombre de usuario debe tener entre 3 y 50 caracteres")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "La contraseña es requerida")]
        [StringLength(500, ErrorMessage = "La contraseña es demasiado larga")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "El email es requerido")]
        [EmailAddress(ErrorMessage = "El formato del email no es válido")]
        [StringLength(100, ErrorMessage = "El email es demasiado largo")]
        public string Email { get; set; } = string.Empty;

        public DateTime RegistrationDate { get; set; }

        public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
        public ICollection<UserMusica> UserMusicas { get; set; } = new List<UserMusica>();
        public Persona? Persona { get; set; }
    }
}
