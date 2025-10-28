namespace Entity.Entities
{
    public class Modulo : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public ICollection<ModuleForm> ModuleForms { get; set; } = new List<ModuleForm>();
    }
}
