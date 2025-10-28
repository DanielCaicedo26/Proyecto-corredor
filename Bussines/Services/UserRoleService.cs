using Entity.Dtos;
using Data.Interfaces;
using Bussines.Interfaces;

namespace Bussines.Services
{
    public class UserRoleService : IUserRoleService
    {
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;

        public UserRoleService(
            IUserRoleRepository userRoleRepository,
            IUserRepository userRepository,
            IRoleRepository roleRepository)
        {
            _userRoleRepository = userRoleRepository;
            _userRepository = userRepository;
            _roleRepository = roleRepository;
        }

        public async Task<bool> AssignRoleToUserAsync(int userId, int roleId)
        {
            if (userId <= 0)
                throw new ArgumentException("User ID debe ser mayor a 0");

            if (roleId <= 0)
                throw new ArgumentException("Role ID debe ser mayor a 0");

            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                throw new KeyNotFoundException("Usuario no encontrado");

            var role = await _roleRepository.GetByIdAsync(roleId);
            if (role == null)
                throw new KeyNotFoundException("Rol no encontrado");

            var hasRole = await _userRoleRepository.UserHasRoleAsync(userId, roleId);
            if (hasRole)
                throw new InvalidOperationException("El usuario ya tiene este rol");

            var userRoleDto = new UserRoleDto { UserId = userId, RoleId = roleId };
            await _userRoleRepository.AddAsync(userRoleDto);

            return true;
        }

        public async Task<bool> RemoveRoleFromUserAsync(int userId, int roleId)
        {
            if (userId <= 0)
                throw new ArgumentException("User ID debe ser mayor a 0");

            if (roleId <= 0)
                throw new ArgumentException("Role ID debe ser mayor a 0");

            var hasRole = await _userRoleRepository.UserHasRoleAsync(userId, roleId);
            if (!hasRole)
                throw new InvalidOperationException("El usuario no tiene este rol");

            return await _userRoleRepository.DeleteAsync(userId, roleId);
        }

        public async Task<List<RoleDto>> GetUserRolesAsync(int userId)
        {
            if (userId <= 0)
                throw new ArgumentException("User ID debe ser mayor a 0");

            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                throw new KeyNotFoundException("Usuario no encontrado");

            var userRoles = await _userRoleRepository.GetRolesByUserAsync(userId);
            
            // Obtener los Roles basados en los RoleIds
            var roleIds = userRoles.Select(ur => ur.RoleId).ToList();
            var roles = new List<RoleDto>();
            
            foreach (var roleId in roleIds)
            {
                var role = await _roleRepository.GetByIdAsync(roleId);
                if (role != null)
                    roles.Add(role);
            }
            
            return roles;
        }

        public async Task<bool> UserHasRoleAsync(int userId, int roleId)
        {
            if (userId <= 0)
                throw new ArgumentException("User ID debe ser mayor a 0");

            if (roleId <= 0)
                throw new ArgumentException("Role ID debe ser mayor a 0");

            return await _userRoleRepository.UserHasRoleAsync(userId, roleId);
        }
    }
}
