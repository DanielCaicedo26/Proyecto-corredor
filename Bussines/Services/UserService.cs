using Entity.Dtos;
using Data.Interfaces;
using Bussines.Interfaces;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;

namespace Bussines.Services
{
    public class UserService : GenericService<UserDto>, IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository) : base(userRepository)
        {
            _userRepository = userRepository;
        }
    }
}
