namespace Entity.Entities
{
    public class ModuleForm : BaseEntity
    {
        public int ModuloId { get; set; }
        public int FormaId { get; set; }
        public Modulo Modulo { get; set; } = null!;
        public Forma Forma { get; set; } = null!;
    }
}
