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
    /// Controller para gestionar permisos de formas en roles
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class RoleFormPermissionsController : ControllerBase
    {
        private readonly IRoleFormPermissionService _roleFormPermissionService;
        private readonly ILogger<RoleFormPermissionsController> _logger;

        public RoleFormPermissionsController(IRoleFormPermissionService roleFormPermissionService, ILogger<RoleFormPermissionsController> logger)
        {
            _roleFormPermissionService = roleFormPermissionService ?? throw new ArgumentNullException(nameof(roleFormPermissionService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Obtiene todos los permisos de formas en roles
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(List<RoleFormPermissionDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<RoleFormPermissionDto>>> GetAll()
        {
            try
            {
                _logger.LogInformation("Obteniendo todos los permisos de formas en roles");
                var roleFormPermissions = await _roleFormPermissionService.GetAllAsync();
                return Ok(roleFormPermissions);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los permisos de formas en roles");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "Error interno del servidor" });
            }
        }

        /// <summary>
        /// Obtiene un permiso de forma en rol por ID
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(RoleFormPermissionDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<RoleFormPermissionDto>> GetById(int id)
        {
            try
            {
                if (id <= 0)
                    return BadRequest("ID debe ser mayor a 0");

                _logger.LogInformation("Obteniendo permiso con ID: {PermissionId}", id);
                var roleFormPermission = await _roleFormPermissionService.GetByIdAsync(id);

                if (roleFormPermission == null)
                {
                    _logger.LogWarning("Permiso no encontrado: {PermissionId}", id);
                    return NotFound("Permiso no encontrado");
                }

                return Ok(roleFormPermission);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener permiso por ID: {PermissionId}", id);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "Error interno del servidor" });
            }
        }

        /// <summary>
        /// Obtiene permisos de un rol y forma específicos
        /// </summary>
        [HttpGet("by-role-form")]
        [ProducesResponseType(typeof(List<RoleFormPermissionDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<RoleFormPermissionDto>>> GetPermissionsByRoleAndForma([FromQuery] int roleId, [FromQuery] int formaId)
        {
            try
            {
                if (roleId <= 0 || formaId <= 0)
                    return BadRequest("IDs de rol y forma deben ser mayores a 0");

                _logger.LogInformation("Obteniendo permisos del rol {RoleId} para la forma {FormaId}", roleId, formaId);
                var permissions = await _roleFormPermissionService.GetPermissionsByRoleAndFormaAsync(roleId, formaId);
                return Ok(permissions);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener permisos por rol y forma");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "Error interno del servidor" });
            }
        }

        /// <summary>
        /// Obtiene permisos de un rol
        /// </summary>
        [HttpGet("by-role/{roleId}")]
        [ProducesResponseType(typeof(List<RoleFormPermissionDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<RoleFormPermissionDto>>> GetPermissionsByRole(int roleId)
        {
            try
            {
                if (roleId <= 0)
                    return BadRequest("ID de rol debe ser mayor a 0");

                _logger.LogInformation("Obteniendo permisos del rol: {RoleId}", roleId);
                var permissions = await _roleFormPermissionService.GetPermissionsByRoleAsync(roleId);
                return Ok(permissions);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener permisos del rol: {RoleId}", roleId);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "Error interno del servidor" });
            }
        }

        /// <summary>
        /// Obtiene permisos de una forma
        /// </summary>
        [HttpGet("by-forma/{formaId}")]
        [ProducesResponseType(typeof(List<RoleFormPermissionDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<RoleFormPermissionDto>>> GetPermissionsByForma(int formaId)
        {
            try
            {
                if (formaId <= 0)
                    return BadRequest("ID de forma debe ser mayor a 0");

                _logger.LogInformation("Obteniendo permisos de la forma: {FormaId}", formaId);
                var permissions = await _roleFormPermissionService.GetPermissionsByFormaAsync(formaId);
                return Ok(permissions);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener permisos de la forma: {FormaId}", formaId);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "Error interno del servidor" });
            }
        }

        /// <summary>
        /// Crea un nuevo permiso de forma en rol
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(RoleFormPermissionDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<RoleFormPermissionDto>> Create([FromBody] RoleFormPermissionDto roleFormPermissionDto)
        {
            try
            {
                if (roleFormPermissionDto == null)
                    return BadRequest("Los datos del permiso son requeridos");

                _logger.LogInformation("Creando nuevo permiso de forma en rol");
                var createdPermission = await _roleFormPermissionService.CreateAsync(roleFormPermissionDto);
                return CreatedAtAction(nameof(GetById), new { id = createdPermission.Id }, createdPermission);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear permiso");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "Error interno del servidor" });
            }
        }

        /// <summary>
        /// Actualiza un permiso de forma en rol
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(RoleFormPermissionDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<RoleFormPermissionDto>> Update(int id, [FromBody] RoleFormPermissionDto roleFormPermissionDto)
        {
            try
            {
                if (id <= 0)
                    return BadRequest("ID debe ser mayor a 0");

                if (roleFormPermissionDto == null)
                    return BadRequest("Los datos del permiso son requeridos");

                roleFormPermissionDto.Id = id;
                _logger.LogInformation("Actualizando permiso con ID: {PermissionId}", id);
                var updatedPermission = await _roleFormPermissionService.UpdateAsync(roleFormPermissionDto);
                
                if (updatedPermission == null)
                {
                    _logger.LogWarning("Permiso no encontrado para actualizar: {PermissionId}", id);
                    return NotFound("Permiso no encontrado");
                }

                return Ok(updatedPermission);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar permiso con ID: {PermissionId}", id);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "Error interno del servidor" });
            }
        }

        /// <summary>
        /// Elimina un permiso de forma en rol
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                if (id <= 0)
                    return BadRequest("ID debe ser mayor a 0");

                _logger.LogInformation("Eliminando permiso con ID: {PermissionId}", id);
                var result = await _roleFormPermissionService.DeleteAsync(id);

                if (!result)
                {
                    _logger.LogWarning("Permiso no encontrado para eliminar: {PermissionId}", id);
                    return NotFound("Permiso no encontrado");
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar permiso con ID: {PermissionId}", id);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "Error interno del servidor" });
            }
        }
    }
}
