namespace Entity.Dtos
{
    public class UserDto : BaseDto
    {
        public int PersonaId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public DateTime RegistrationDate { get; set; }
    }
}
