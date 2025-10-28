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
    /// Controller para gestionar Permisos
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class PermissionsController : ControllerBase
    {
        private readonly IPermissionService _permissionService;
        private readonly ILogger<PermissionsController> _logger;

        public PermissionsController(IPermissionService permissionService, ILogger<PermissionsController> logger)
        {
            _permissionService = permissionService ?? throw new ArgumentNullException(nameof(permissionService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Obtiene todos los permisos
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(List<PermissionDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<PermissionDto>>> GetAll()
        {
            try
            {
                _logger.LogInformation("Obteniendo todos los permisos");
                var permissions = await _permissionService.GetAllAsync();
                return Ok(permissions);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los permisos");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "Error interno del servidor" });
            }
        }

        /// <summary>
        /// Obtiene un permiso por ID
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(PermissionDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PermissionDto>> GetById(int id)
        {
            try
            {
                if (id <= 0)
                    return BadRequest("ID debe ser mayor a 0");

                _logger.LogInformation("Obteniendo permiso con ID: {PermissionId}", id);
                var permission = await _permissionService.GetByIdAsync(id);

                if (permission == null)
                {
                    _logger.LogWarning("Permiso no encontrado: {PermissionId}", id);
                    return NotFound("Permiso no encontrado");
                }

                return Ok(permission);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener permiso por ID: {PermissionId}", id);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "Error interno del servidor" });
            }
        }

        /// <summary>
        /// Obtiene un permiso por nombre
        /// </summary>
        [HttpGet("by-name/{name}")]
        [ProducesResponseType(typeof(PermissionDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PermissionDto>> GetByName(string name)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(name))
                    return BadRequest("El nombre del permiso es requerido");

                _logger.LogInformation("Obteniendo permiso con nombre: {PermissionName}", name);
                var permission = await _permissionService.GetByNameAsync(name);

                if (permission == null)
                {
                    _logger.LogWarning("Permiso no encontrado: {PermissionName}", name);
                    return NotFound("Permiso no encontrado");
                }

                return Ok(permission);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener permiso por nombre: {PermissionName}", name);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "Error interno del servidor" });
            }
        }

        /// <summary>
        /// Obtiene permisos por rol
        /// </summary>
        [HttpGet("by-role/{roleId}")]
        [ProducesResponseType(typeof(List<PermissionDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<PermissionDto>>> GetPermissionsByRole(int roleId)
        {
            try
            {
                if (roleId <= 0)
                    return BadRequest("ID de rol debe ser mayor a 0");

                _logger.LogInformation("Obteniendo permisos del rol: {RoleId}", roleId);
                var permissions = await _permissionService.GetPermissionsByRoleAsync(roleId);
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
        /// Crea un nuevo permiso
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(PermissionDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PermissionDto>> Create([FromBody] PermissionDto permissionDto)
        {
            try
            {
                if (permissionDto == null)
                    return BadRequest("Los datos del permiso son requeridos");

                if (string.IsNullOrWhiteSpace(permissionDto.Name))
                    return BadRequest("El nombre del permiso es requerido");

                _logger.LogInformation("Creando nuevo permiso: {PermissionName}", permissionDto.Name);
                var createdPermission = await _permissionService.CreateAsync(permissionDto);
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
        /// Actualiza un permiso existente
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(PermissionDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PermissionDto>> Update(int id, [FromBody] PermissionDto permissionDto)
        {
            try
            {
                if (id <= 0)
                    return BadRequest("ID debe ser mayor a 0");

                if (permissionDto == null)
                    return BadRequest("Los datos del permiso son requeridos");

                permissionDto.Id = id;
                _logger.LogInformation("Actualizando permiso con ID: {PermissionId}", id);
                var updatedPermission = await _permissionService.UpdateAsync(permissionDto);
                
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
        /// Elimina un permiso
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
                var result = await _permissionService.DeleteAsync(id);

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
