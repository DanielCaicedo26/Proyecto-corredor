using Entity.Dtos;
using Data.Interfaces;
using Bussines.Interfaces;

namespace Bussines.Services
{
    public class UserRoleService : GenericService<UserRoleDto>, IUserRoleService
    {
        private readonly IUserRoleRepository _userRoleRepository;

        public UserRoleService(IUserRoleRepository userRoleRepository) : base(userRoleRepository)
        {
            _userRoleRepository = userRoleRepository;
        }
    }
}
