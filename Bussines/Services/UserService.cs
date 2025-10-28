using Entity.Dtos;
using Data.Interfaces;
using Bussines.Interfaces;

namespace Bussines.Services
{
    public class UserService : GenericService<UserDto>, IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository) : base(userRepository)
        {
            _userRepository = userRepository;
        }

        public override async Task<UserDto> CreateAsync(UserDto dto)
        {
            if (dto == null)
                throw new ArgumentException("El usuario no puede ser nulo");

            ValidateData(dto);

            var existing = await _userRepository.GetByEmailAsync(dto.Email);
            if (existing != null)
                throw new InvalidOperationException("Este email ya está registrado");

            var existingUsername = await _userRepository.GetByUsernameAsync(dto.Username);
            if (existingUsername != null)
                throw new InvalidOperationException("Este nombre de usuario ya existe");

            return await _userRepository.AddAsync(dto);
        }

        public override async Task<UserDto> UpdateAsync(UserDto dto)
        {
            if (dto == null)
                throw new ArgumentException("El usuario no puede ser nulo");

            if (dto.Id <= 0)
                throw new ArgumentException("ID debe ser mayor a 0");

            var existing = await _userRepository.GetByIdAsync(dto.Id);
            if (existing == null)
                throw new KeyNotFoundException("Usuario no encontrado");

            ValidateData(dto);

            return await _userRepository.UpdateAsync(dto);
        }

        public async Task<UserDto> GetByEmailAsync(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email es requerido");

            var user = await _userRepository.GetByEmailAsync(email);
            if (user == null)
                throw new KeyNotFoundException("Usuario no encontrado");

            return user;
        }

        public async Task<UserDto> GetByUsernameAsync(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentException("Username es requerido");

            var user = await _userRepository.GetByUsernameAsync(username);
            if (user == null)
                throw new KeyNotFoundException("Usuario no encontrado");

            return user;
        }

        public async Task<List<UserDto>> GetUsersByRoleAsync(int roleId)
        {
            if (roleId <= 0)
                throw new ArgumentException("Role ID debe ser mayor a 0");

            return await _userRepository.GetUsersByRoleAsync(roleId);
        }

        protected override void ValidateData(UserDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Username))
                throw new ArgumentException("Username es requerido");

            if (dto.Username.Length < 3)
                throw new ArgumentException("Username debe tener mínimo 3 caracteres");

            if (string.IsNullOrWhiteSpace(dto.Email))
                throw new ArgumentException("Email es requerido");

            if (!IsValidEmail(dto.Email))
                throw new ArgumentException("Email no es válido");
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}
