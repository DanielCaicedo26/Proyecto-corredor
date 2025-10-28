namespace Entity.Entities
{
    public class User : BaseEntity
    {
        public int? PersonaId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public DateTime RegistrationDate { get; set; }
        public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
        public Persona? Persona { get; set; }
    }
}
