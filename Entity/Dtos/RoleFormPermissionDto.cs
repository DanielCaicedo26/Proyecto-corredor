namespace Entity.Dtos
{
    public class RoleFormPermissionDto : BaseDto
    {
        public int RoleId { get; set; }
        public int FormaId { get; set; }
        public int PermissionId { get; set; }
    }
}
