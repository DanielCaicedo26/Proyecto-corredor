using Entity.Dtos;
using Data.Interfaces;
using Bussines.Interfaces;

namespace Bussines.Services
{
    public class PermissionService : GenericService<PermissionDto>, IPermissionService
    {
        private readonly IPermissionRepository _permissionRepository;

        public PermissionService(IPermissionRepository permissionRepository) : base(permissionRepository)
        {
            _permissionRepository = permissionRepository;
        }
    }
}
