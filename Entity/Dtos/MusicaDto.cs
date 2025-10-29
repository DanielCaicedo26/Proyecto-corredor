namespace Entity.Dtos
{
    public class MusicaDto : BaseDto
    {
        public string? Titulo { get; set; }
        public string? Artista { get; set; }
        public string? Album { get; set; }
        public string? Genero { get; set; }
        public TimeSpan Duracion { get; set; }
        public string? UrlCancion { get; set; }
        public int UserId { get; set; }
    }
}
