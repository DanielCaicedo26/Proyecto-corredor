using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Entity.Dtos;
using Bussines.Interfaces;

namespace Modelo_de_security.Controllers
{
    /// <summary>
    /// Controller para gestionar roles de usuarios
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class UserRolesController : ControllerBase
    {
        private readonly IUserRoleService _userRoleService;
        private readonly ILogger<UserRolesController> _logger;

        public UserRolesController(IUserRoleService userRoleService, ILogger<UserRolesController> logger)
        {
            _userRoleService = userRoleService ?? throw new ArgumentNullException(nameof(userRoleService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Obtiene los roles de un usuario
        /// </summary>
        [HttpGet("user/{userId}")]
        [ProducesResponseType(typeof(List<RoleDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<RoleDto>>> GetUserRoles(int userId)
        {
            try
            {
                if (userId <= 0)
                    return BadRequest("ID de usuario debe ser mayor a 0");

                _logger.LogInformation("Obteniendo roles del usuario: {UserId}", userId);
                var roles = await _userRoleService.GetUserRolesAsync(userId);
                return Ok(roles);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener roles del usuario: {UserId}", userId);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "Error interno del servidor" });
            }
        }

        /// <summary>
        /// Verifica si un usuario tiene un rol específico
        /// </summary>
        [HttpGet("user/{userId}/role/{roleId}/check")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<bool>> CheckUserHasRole(int userId, int roleId)
        {
            try
            {
                if (userId <= 0 || roleId <= 0)
                    return BadRequest("IDs de usuario y rol deben ser mayores a 0");

                _logger.LogInformation("Verificando si usuario {UserId} tiene rol {RoleId}", userId, roleId);
                var hasRole = await _userRoleService.UserHasRoleAsync(userId, roleId);
                return Ok(hasRole);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al verificar rol del usuario: {UserId}", userId);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "Error interno del servidor" });
            }
        }

        /// <summary>
        /// Asigna un rol a un usuario
        /// </summary>
        [HttpPost("assign")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AssignRoleToUser([FromQuery] int userId, [FromQuery] int roleId)
        {
            try
            {
                if (userId <= 0 || roleId <= 0)
                    return BadRequest("IDs de usuario y rol deben ser mayores a 0");

                _logger.LogInformation("Asignando rol {RoleId} al usuario {UserId}", roleId, userId);
                var result = await _userRoleService.AssignRoleToUserAsync(userId, roleId);

                if (!result)
                {
                    _logger.LogWarning("No se pudo asignar el rol {RoleId} al usuario {UserId}", roleId, userId);
                    return BadRequest("No se pudo asignar el rol al usuario");
                }

                return Ok(new { message = "Rol asignado correctamente" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al asignar rol al usuario");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "Error interno del servidor" });
            }
        }

        /// <summary>
        /// Remueve un rol de un usuario
        /// </summary>
        [HttpPost("remove")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RemoveRoleFromUser([FromQuery] int userId, [FromQuery] int roleId)
        {
            try
            {
                if (userId <= 0 || roleId <= 0)
                    return BadRequest("IDs de usuario y rol deben ser mayores a 0");

                _logger.LogInformation("Removiendo rol {RoleId} del usuario {UserId}", roleId, userId);
                var result = await _userRoleService.RemoveRoleFromUserAsync(userId, roleId);

                if (!result)
                {
                    _logger.LogWarning("No se pudo remover el rol {RoleId} del usuario {UserId}", roleId, userId);
                    return BadRequest("No se pudo remover el rol del usuario");
                }

                return Ok(new { message = "Rol removido correctamente" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al remover rol del usuario");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "Error interno del servidor" });
            }
        }
    }
}
