namespace Entity.Entities
{
    public class Permission : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public ICollection<RoleFormPermission> RoleFormPermissions { get; set; } = new List<RoleFormPermission>();
    }
}
