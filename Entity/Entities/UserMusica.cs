namespace Entity.Entities
{
    public class UserMusica
    {
        public int UserId { get; set; }
        public int MusicaId { get; set; }
        public DateTime FechaAgregada { get; set; } = DateTime.Now;

        // Navegaciones
        public User? User { get; set; }
        public Musica? Musica { get; set; }
    }
}
