using Entity.Dtos;
using Data.Interfaces;
using Bussines.Interfaces;

namespace Bussines.Services
{
    public class RoleService : GenericService<RoleDto>, IRoleService
    {
        private readonly IRoleRepository _roleRepository;

        public RoleService(IRoleRepository roleRepository) : base(roleRepository)
        {
            _roleRepository = roleRepository;
        }
    }
}
