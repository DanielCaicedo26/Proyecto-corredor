using Entity.Dtos;
using Entity.Dtos.Auth;
using Data.Interfaces;
using Bussines.Interfaces;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Text.RegularExpressions;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Logging;
using BCrypt.Net;

namespace Bussines.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPersonaService _personaService;
        private readonly IJwtSettings _jwtSettings;
        private readonly ILogger<AuthService> _logger;

        public AuthService(
            IUserRepository userRepository,
            IPersonaService personaService,
            IJwtSettings jwtSettings, 
            ILogger<AuthService> logger)
        {
            _userRepository = userRepository;
            _personaService = personaService;
            _jwtSettings = jwtSettings;
            _logger = logger;
        }

        /// <summary>
        /// Autentica un usuario con username y contraseña
        /// </summary>
        public async Task<AuthResponse> LoginAsync(LoginRequest request)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request?.Username) || string.IsNullOrWhiteSpace(request?.Password))
                {
                    return new AuthResponse
                    {
                        Success = false,
                        Message = "Username y Password son requeridos"
                    };
                }

                // Buscar usuario por username
                var user = await _userRepository.GetByUsernameAsync(request.Username);
                if (user == null)
                {
                    _logger.LogWarning("Intento de login fallido: usuario '{Username}' no encontrado", request.Username);
                    return new AuthResponse
                    {
                        Success = false,
                        Message = "Usuario o contraseña incorrectos"
                    };
                }

                // Verificar contraseña
                if (string.IsNullOrEmpty(user.Password) || !VerifyPassword(request.Password, user.Password))
                {
                    _logger.LogWarning("Intento de login fallido: contraseña incorrecta para usuario '{Username}'", request.Username);
                    return new AuthResponse
                    {
                        Success = false,
                        Message = "Usuario o contraseña incorrectos"
                    };
                }

                // Generar JWT
                var token = GenerateJwtToken(user.Id, user.Username ?? string.Empty);

                _logger.LogInformation("Usuario '{Username}' ha iniciado sesión exitosamente", request.Username);

                return new AuthResponse
                {
                    Success = true,
                    Message = "Login exitoso",
                    Token = token,
                    User = user
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error durante el login");
                return new AuthResponse
                {
                    Success = false,
                    Message = "Error interno del servidor"
                };
            }
        }

        /// <summary>
        /// Registra un nuevo usuario
        /// </summary>
        public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
        {
            try
            {
                // Validaciones
                if (string.IsNullOrWhiteSpace(request?.Username))
                {
                    return new AuthResponse
                    {
                        Success = false,
                        Message = "Username es requerido"
                    };
                }

                if (request.Username.Length < 3)
                {
                    return new AuthResponse
                    {
                        Success = false,
                        Message = "Username debe tener mínimo 3 caracteres"
                    };
                }

                if (string.IsNullOrWhiteSpace(request?.Email))
                {
                    return new AuthResponse
                    {
                        Success = false,
                        Message = "Email es requerido"
                    };
                }

                if (!IsValidEmail(request.Email))
                {
                    return new AuthResponse
                    {
                        Success = false,
                        Message = "Email no es válido"
                    };
                }

                if (string.IsNullOrWhiteSpace(request?.Password))
                {
                    return new AuthResponse
                    {
                        Success = false,
                        Message = "Password es requerido"
                    };
                }

                if (request.Password.Length < 6)
                {
                    return new AuthResponse
                    {
                        Success = false,
                        Message = "Password debe tener mínimo 6 caracteres"
                    };
                }

                if (request.Password != request.ConfirmPassword)
                {
                    return new AuthResponse
                    {
                        Success = false,
                        Message = "Las contraseñas no coinciden"
                    };
                }

                // Verificar si username ya existe
                var existingUsername = await _userRepository.GetByUsernameAsync(request.Username);
                if (existingUsername != null)
                {
                    return new AuthResponse
                    {
                        Success = false,
                        Message = "Este username ya está registrado"
                    };
                }

                // Verificar si email ya existe
                var existingEmail = await _userRepository.GetByEmailAsync(request.Email);
                if (existingEmail != null)
                {
                    return new AuthResponse
                    {
                        Success = false,
                        Message = "Este email ya está registrado"
                    };
                }

                // Crear nuevo usuario (nota: la contraseña será hasheada en el repositorio/entity)
                var newUser = new UserDto
                {
                    Username = request.Username,
                    Email = request.Email,
                    Password = HashPassword(request.Password),
                    RegistrationDate = DateTime.UtcNow
                };

                var createdUser = await _userRepository.AddAsync(newUser);

                _logger.LogInformation("Nuevo usuario registrado: '{Username}'", request.Username);

                return new AuthResponse
                {
                    Success = true,
                    Message = "Registro exitoso",
                    User = createdUser
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error durante el registro");
                return new AuthResponse
                {
                    Success = false,
                    Message = "Error interno del servidor"
                };
            }
        }

        /// <summary>
        /// Registra un nuevo usuario con datos de Persona
        /// </summary>
        public async Task<AuthResponse> RegisterWithPersonaAsync(RegisterRequestExtended request)
        {
            try
            {
                // Validaciones básicas
                if (string.IsNullOrWhiteSpace(request?.Username))
                {
                    return new AuthResponse
                    {
                        Success = false,
                        Message = "Username es requerido"
                    };
                }

                if (request.Username.Length < 3)
                {
                    return new AuthResponse
                    {
                        Success = false,
                        Message = "Username debe tener mínimo 3 caracteres"
                    };
                }

                if (string.IsNullOrWhiteSpace(request?.Email))
                {
                    return new AuthResponse
                    {
                        Success = false,
                        Message = "Email es requerido"
                    };
                }

                if (!IsValidEmail(request.Email))
                {
                    return new AuthResponse
                    {
                        Success = false,
                        Message = "Email no es válido"
                    };
                }

                if (string.IsNullOrWhiteSpace(request?.Password))
                {
                    return new AuthResponse
                    {
                        Success = false,
                        Message = "Password es requerido"
                    };
                }

                if (request.Password.Length < 6)
                {
                    return new AuthResponse
                    {
                        Success = false,
                        Message = "Password debe tener mínimo 6 caracteres"
                    };
                }

                if (request.Password != request.ConfirmPassword)
                {
                    return new AuthResponse
                    {
                        Success = false,
                        Message = "Las contraseñas no coinciden"
                    };
                }

                // Validaciones de Persona
                if (string.IsNullOrWhiteSpace(request?.Name))
                {
                    return new AuthResponse
                    {
                        Success = false,
                        Message = "Nombre es requerido"
                    };
                }

                if (string.IsNullOrWhiteSpace(request?.LastName))
                {
                    return new AuthResponse
                    {
                        Success = false,
                        Message = "Apellido es requerido"
                    };
                }

                if (string.IsNullOrWhiteSpace(request?.Phone))
                {
                    return new AuthResponse
                    {
                        Success = false,
                        Message = "Teléfono es requerido"
                    };
                }

                if (string.IsNullOrWhiteSpace(request?.DocumentNumber))
                {
                    return new AuthResponse
                    {
                        Success = false,
                        Message = "Número de documento es requerido"
                    };
                }

                // Verificar si username ya existe
                var existingUsername = await _userRepository.GetByUsernameAsync(request.Username);
                if (existingUsername != null)
                {
                    return new AuthResponse
                    {
                        Success = false,
                        Message = "Este username ya está registrado"
                    };
                }

                // Verificar si email ya existe
                var existingEmail = await _userRepository.GetByEmailAsync(request.Email);
                if (existingEmail != null)
                {
                    return new AuthResponse
                    {
                        Success = false,
                        Message = "Este email ya está registrado"
                    };
                }

                // ✅ Crear la Persona primero
                var newPersona = new PersonaDto
                {
                    Name = request.Name,
                    LastName = request.LastName,
                    Phone = request.Phone,
                    DocumentNumber = request.DocumentNumber
                };

                PersonaDto createdPersona;
                try
                {
                    createdPersona = await _personaService.CreateAsync(newPersona);
                }
                catch (InvalidOperationException ex)
                {
                    _logger.LogWarning("Error al crear Persona durante registro: {ErrorMessage}", ex.Message);
                    return new AuthResponse
                    {
                        Success = false,
                        Message = ex.Message
                    };
                }

                if (createdPersona == null || createdPersona.Id <= 0)
                {
                    _logger.LogError("No se pudo crear la Persona para el nuevo usuario");
                    return new AuthResponse
                    {
                        Success = false,
                        Message = "Error al crear el perfil de persona"
                    };
                }

                // ✅ Crear el Usuario asociado a la Persona
                var newUser = new UserDto
                {
                    Username = request.Username,
                    Email = request.Email,
                    Password = HashPassword(request.Password),
                    PersonaId = createdPersona.Id,
                    RegistrationDate = DateTime.UtcNow
                };

                var createdUser = await _userRepository.AddAsync(newUser);

                if (createdUser == null || createdUser.Id <= 0)
                {
                    _logger.LogError("No se pudo crear el Usuario para el registro");
                    return new AuthResponse
                    {
                        Success = false,
                        Message = "Error al crear la cuenta de usuario"
                    };
                }

                // ✅ Generar JWT Token para el nuevo usuario
                var token = GenerateJwtToken(createdUser.Id, createdUser.Username ?? string.Empty);

                _logger.LogInformation("Nuevo usuario registrado con Persona: '{Username}' (PersonaId: {PersonaId})", 
                    request.Username, createdPersona.Id);

                return new AuthResponse
                {
                    Success = true,
                    Message = "Registro exitoso. Bienvenido!",
                    Token = token,
                    User = createdUser
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error durante el registro con Persona");
                return new AuthResponse
                {
                    Success = false,
                    Message = "Error interno del servidor"
                };
            }
        }
        /// Genera un JWT token
        /// </summary>
        public string GenerateJwtToken(int userId, string username)
        {
            try
            {
                if (string.IsNullOrEmpty(username))
                    throw new ArgumentException("Username no puede ser nulo o vacío");

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
                var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var claims = new[]
                {
                    new System.Security.Claims.Claim("sub", userId.ToString()),
                    new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Name, username),
                    new System.Security.Claims.Claim("iat", DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString())
                };

                var token = new JwtSecurityToken(
                    issuer: _jwtSettings.Issuer,
                    audience: _jwtSettings.Audience,
                    claims: claims,
                    expires: DateTime.UtcNow.AddMinutes(_jwtSettings.ExpirationMinutes),
                    signingCredentials: credentials
                );

                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al generar JWT token");
                throw;
            }
        }

        /// <summary>
        /// Hashea una contraseña usando BCrypt
        /// </summary>
        private string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        /// <summary>
        /// Verifica una contraseña contra su hash usando BCrypt
        /// </summary>
        private bool VerifyPassword(string password, string hash)
        {
            try
            {
                return BCrypt.Net.BCrypt.Verify(password, hash);
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Valida formato de email usando regex
        /// </summary>
        private bool IsValidEmail(string email)
        {
            try
            {
                // Patrón regex para validar email
                const string emailPattern = @"^[^\s@]+@[^\s@]+\.[^\s@]+$";
                
                if (string.IsNullOrWhiteSpace(email))
                    return false;

                // Validar con regex
                if (!Regex.IsMatch(email, emailPattern))
                    return false;

                // Validar con MailAddress como doble verificación
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
