namespace Entity.Entities
{
    public class RoleFormPermission : BaseEntity
    {
        public int RoleId { get; set; }
        public int FormaId { get; set; }
        public int PermissionId { get; set; }
        public Role Role { get; set; } = null!;
        public Forma Forma { get; set; } = null!;
        public Permission Permission { get; set; } = null!;
    }
}
