namespace Entity.Entities
{
    public class Musica : BaseEntity
    {
        public string Titulo { get; set; }
        public string Artista { get; set; }
        public string Album { get; set; }
        public string Genero { get; set; }
        public TimeSpan Duracion { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
