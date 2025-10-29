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
        private readonly ILogger<UserService> _logger;

        public UserService(IUserRepository userRepository, ILogger<UserService> logger) : base(userRepository)
        {
            _userRepository = userRepository;
            _logger = logger;
        }

        public override async Task<UserDto> CreateAsync(UserDto dto)
        {
            try
            {
                if (dto == null)
                    throw new ArgumentException("El usuario no puede ser nulo");

                _logger.LogInformation("Iniciando creación de nuevo usuario: {Username}", dto.Username);
                ValidateData(dto);

                var existing = await _userRepository.GetByEmailAsync(dto.Email!);
                if (existing != null)
                {
                    _logger.LogWarning("Intento de crear usuario con email duplicado: {Email}", dto.Email);
                    throw new InvalidOperationException("Este email ya está registrado");
                }

                var existingUsername = await _userRepository.GetByUsernameAsync(dto.Username!);
                if (existingUsername != null)
                {
                    _logger.LogWarning("Intento de crear usuario con username duplicado: {Username}", dto.Username);
                    throw new InvalidOperationException("Este nombre de usuario ya existe");
                }

                var result = await _userRepository.AddAsync(dto);
                _logger.LogInformation("Usuario creado exitosamente. UserId: {UserId}, Username: {Username}", result.Id, result.Username);
                return result;
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Error de validación al crear usuario");
                throw;
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Operación inválida al crear usuario");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al crear usuario: {Message}", ex.Message);
                throw;
            }
        }

        public override async Task<UserDto> UpdateAsync(UserDto dto)
        {
            try
            {
                if (dto == null)
                    throw new ArgumentException("El usuario no puede ser nulo");

                if (dto.Id <= 0)
                    throw new ArgumentException("ID debe ser mayor a 0");

                _logger.LogInformation("Iniciando actualización de usuario: UserId={UserId}, Username={Username}", dto.Id, dto.Username);

                var existing = await _userRepository.GetByIdAsync(dto.Id);
                if (existing == null)
                {
                    _logger.LogWarning("Usuario no encontrado para actualizar. UserId: {UserId}", dto.Id);
                    throw new KeyNotFoundException("Usuario no encontrado");
                }

                ValidateData(dto);

                var result = await _userRepository.UpdateAsync(dto);
                _logger.LogInformation("Usuario actualizado exitosamente. UserId: {UserId}", result.Id);
                return result;
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Error de validación al actualizar usuario");
                throw;
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, "Usuario no encontrado para actualizar");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al actualizar usuario: {Message}", ex.Message);
                throw;
            }
        }

        public override async Task<bool> DeleteAsync(int id)
        {
            try
            {
                if (id <= 0)
                    throw new ArgumentException("ID debe ser mayor a 0");

                _logger.LogInformation("Iniciando eliminación de usuario: UserId={UserId}", id);

                var existing = await _userRepository.GetByIdAsync(id);
                if (existing == null)
                {
                    _logger.LogWarning("Usuario no encontrado para eliminar. UserId: {UserId}", id);
                    throw new KeyNotFoundException("Usuario no encontrado");
                }

                await _userRepository.DeleteAsync(id);
                _logger.LogInformation("Usuario eliminado exitosamente. UserId: {UserId}, Username: {Username}", id, existing.Username);
                return true;
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Error de validación al eliminar usuario");
                throw;
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, "Usuario no encontrado para eliminar");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al eliminar usuario: {Message}", ex.Message);
                throw;
            }
        }

        protected override void ValidateData(UserDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Username))
                throw new ArgumentException("Username es requerido");

            if (dto.Username.Length < 3)
                throw new ArgumentException("Username debe tener mínimo 3 caracteres");

            if (dto.Username.Length > 50)
                throw new ArgumentException("Username debe tener máximo 50 caracteres");

            if (string.IsNullOrWhiteSpace(dto.Email))
                throw new ArgumentException("Email es requerido");

            if (!IsValidEmail(dto.Email))
                throw new ArgumentException("Email no es válido");
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                const string emailPattern = @"^[^\s@]+@[^\s@]+\.[^\s@]+$";
                if (string.IsNullOrWhiteSpace(email))
                    return false;

                if (!Regex.IsMatch(email, emailPattern))
                    return false;

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
