using Entity.Dtos;
using Data.Interfaces;
using Bussines.Interfaces;

namespace Bussines.Services
{
    public class RoleFormPermissionService : GenericService<RoleFormPermissionDto>, IRoleFormPermissionService
    {
        private readonly IRoleFormPermissionRepository _roleFormPermissionRepository;

        public RoleFormPermissionService(IRoleFormPermissionRepository roleFormPermissionRepository) : base(roleFormPermissionRepository)
        {
            _roleFormPermissionRepository = roleFormPermissionRepository;
        }
    }
}
