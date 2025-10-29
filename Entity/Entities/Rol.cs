using System.ComponentModel.DataAnnotations;

namespace Entity.Entities
{
    public class Role : BaseEntity
    {
        [Required(ErrorMessage = "El nombre del rol es requerido")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "El nombre del rol debe tener entre 2 y 50 caracteres")]
        public string Name { get; set; } = string.Empty;

        [StringLength(200, ErrorMessage = "La descripción no puede exceder 200 caracteres")]
        public string Description { get; set; } = string.Empty;

        public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    }
}
