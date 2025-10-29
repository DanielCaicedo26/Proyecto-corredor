namespace Entity.Entities
{
    public class Musica : BaseEntity
    {
        public string Titulo { get; set; }
        public string Artista { get; set; }
        public string Album { get; set; }
        public string Genero { get; set; }
        public TimeSpan Duracion { get; set; }

        // Relación muchos a muchos con User
        public ICollection<UserMusica> UserMusicas { get; set; } = new List<UserMusica>();
    }
}
