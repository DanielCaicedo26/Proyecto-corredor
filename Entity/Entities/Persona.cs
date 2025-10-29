using System.ComponentModel.DataAnnotations;

namespace Entity.Entities
{
    public class Persona : BaseEntity
    {
        [Required(ErrorMessage = "El nombre es requerido")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "El nombre debe tener entre 2 y 50 caracteres")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "El apellido es requerido")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "El apellido debe tener entre 2 y 50 caracteres")]
        public string LastName { get; set; } = string.Empty;

        [Phone(ErrorMessage = "El formato del teléfono no es válido")]
        [StringLength(20, ErrorMessage = "El teléfono no puede exceder 20 caracteres")]
        public string Phone { get; set; } = string.Empty;

        [Required(ErrorMessage = "El número de documento es requerido")]
        [StringLength(20, MinimumLength = 5, ErrorMessage = "El número de documento debe tener entre 5 y 20 caracteres")]
        public string DocumentNumber { get; set; } = string.Empty;

        public ICollection<User> Users { get; set; } = new List<User>();
    }
}
