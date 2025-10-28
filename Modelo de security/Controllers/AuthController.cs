using Microsoft.AspNetCore.Mvc;
using Entity.Dtos.Auth;
using Bussines.Interfaces;

namespace Modelo_de_security.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IAuthService authService, ILogger<AuthController> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        /// <summary>
        /// Login de usuario con username y password
        /// </summary>
        /// <remarks>
        /// Ejemplo de solicitud:
        /// 
        ///     POST /api/auth/login
        ///     {
        ///         "username": "user123",
        ///         "password": "password123"
        ///     }
        /// 
        /// Respuesta exitosa (200):
        ///     {
        ///         "success": true,
        ///         "message": "Login exitoso",
        ///         "token": "eyJhbGciOiJIUzI1NiIs...",
        ///         "user": {
        ///             "id": 1,
        ///             "username": "user123",
        ///             "email": "user@example.com"
        ///         }
        ///     }
        /// </remarks>
        [HttpPost("login")]
        [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<AuthResponse>> Login([FromBody] LoginRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new AuthResponse
                {
                    Success = false,
                    Message = "Datos de entrada no válidos"
                });
            }

            var result = await _authService.LoginAsync(request);

            if (!result.Success)
            {
                return Unauthorized(result);
            }

            return Ok(result);
        }

        /// <summary>
        /// Registro de nuevo usuario
        /// </summary>
        /// <remarks>
        /// Ejemplo de solicitud:
        /// 
        ///     POST /api/auth/register
        ///     {
        ///         "username": "user123",
        ///         "email": "user@example.com",
        ///         "password": "password123",
        ///         "confirmPassword": "password123"
        ///     }
        /// 
        /// Respuesta exitosa (201):
        ///     {
        ///         "success": true,
        ///         "message": "Registro exitoso",
        ///         "user": {
        ///             "id": 1,
        ///             "username": "user123",
        ///             "email": "user@example.com"
        ///         }
        ///     }
        /// </remarks>
        [HttpPost("register")]
        [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<AuthResponse>> Register([FromBody] RegisterRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new AuthResponse
                {
                    Success = false,
                    Message = "Datos de entrada no válidos"
                });
            }

            var result = await _authService.RegisterAsync(request);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return CreatedAtAction(nameof(Login), result);
        }
    }
}
