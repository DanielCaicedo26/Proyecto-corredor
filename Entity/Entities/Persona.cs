namespace Entity.Entities
{
    public class Persona : BaseEntity
    {
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string DocumentNumber { get; set; }
        public ICollection<User> Users { get; set; } = new List<User>();
    }
}
