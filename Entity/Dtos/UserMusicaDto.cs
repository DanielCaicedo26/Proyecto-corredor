namespace Entity.Dtos
{
    public class UserMusicaDto : BaseDto
    {
        public int UserId { get; set; }
        public int MusicaId { get; set; }
        public DateTime FechaAgregada { get; set; }
    }
}
