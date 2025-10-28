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
    /// Controller para gestionar Roles
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class RolesController : ControllerBase
    {
        private readonly IRoleService _roleService;
        private readonly ILogger<RolesController> _logger;

        public RolesController(IRoleService roleService, ILogger<RolesController> logger)
        {
            _roleService = roleService ?? throw new ArgumentNullException(nameof(roleService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Obtiene todos los roles
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(List<RoleDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<RoleDto>>> GetAll()
        {
            try
            {
                _logger.LogInformation("Obteniendo todos los roles");
                var roles = await _roleService.GetAllAsync();
                return Ok(roles);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los roles");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "Error interno del servidor" });
            }
        }

        /// <summary>
        /// Obtiene un rol por ID
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(RoleDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<RoleDto>> GetById(int id)
        {
            try
            {
                if (id <= 0)
                    return BadRequest("ID debe ser mayor a 0");

                _logger.LogInformation("Obteniendo rol con ID: {RoleId}", id);
                var role = await _roleService.GetByIdAsync(id);

                if (role == null)
                {
                    _logger.LogWarning("Rol no encontrado: {RoleId}", id);
                    return NotFound("Rol no encontrado");
                }

                return Ok(role);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener rol por ID: {RoleId}", id);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "Error interno del servidor" });
            }
        }

        /// <summary>
        /// Obtiene un rol por nombre
        /// </summary>
        [HttpGet("by-name/{name}")]
        [ProducesResponseType(typeof(RoleDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<RoleDto>> GetByName(string name)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(name))
                    return BadRequest("El nombre del rol es requerido");

                _logger.LogInformation("Obteniendo rol con nombre: {RoleName}", name);
                var role = await _roleService.GetByNameAsync(name);

                if (role == null)
                {
                    _logger.LogWarning("Rol no encontrado: {RoleName}", name);
                    return NotFound("Rol no encontrado");
                }

                return Ok(role);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener rol por nombre: {RoleName}", name);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "Error interno del servidor" });
            }
        }

        /// <summary>
        /// Obtiene roles de un usuario
        /// </summary>
        [HttpGet("by-user/{userId}")]
        [ProducesResponseType(typeof(List<RoleDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<RoleDto>>> GetRolesByUser(int userId)
        {
            try
            {
                if (userId <= 0)
                    return BadRequest("ID de usuario debe ser mayor a 0");

                _logger.LogInformation("Obteniendo roles del usuario: {UserId}", userId);
                var roles = await _roleService.GetRolesByUserAsync(userId);
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
        /// Crea un nuevo rol
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(RoleDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<RoleDto>> Create([FromBody] RoleDto roleDto)
        {
            try
            {
                if (roleDto == null)
                    return BadRequest("Los datos del rol son requeridos");

                if (string.IsNullOrWhiteSpace(roleDto.Name))
                    return BadRequest("El nombre del rol es requerido");

                _logger.LogInformation("Creando nuevo rol: {RoleName}", roleDto.Name);
                var createdRole = await _roleService.CreateAsync(roleDto);
                return CreatedAtAction(nameof(GetById), new { id = createdRole.Id }, createdRole);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear rol");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "Error interno del servidor" });
            }
        }

        /// <summary>
        /// Actualiza un rol existente
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(RoleDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<RoleDto>> Update(int id, [FromBody] RoleDto roleDto)
        {
            try
            {
                if (id <= 0)
                    return BadRequest("ID debe ser mayor a 0");

                if (roleDto == null)
                    return BadRequest("Los datos del rol son requeridos");

                roleDto.Id = id;
                _logger.LogInformation("Actualizando rol con ID: {RoleId}", id);
                var updatedRole = await _roleService.UpdateAsync(roleDto);
                
                if (updatedRole == null)
                {
                    _logger.LogWarning("Rol no encontrado para actualizar: {RoleId}", id);
                    return NotFound("Rol no encontrado");
                }

                return Ok(updatedRole);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar rol con ID: {RoleId}", id);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "Error interno del servidor" });
            }
        }

        /// <summary>
        /// Elimina un rol
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

                _logger.LogInformation("Eliminando rol con ID: {RoleId}", id);
                var result = await _roleService.DeleteAsync(id);

                if (!result)
                {
                    _logger.LogWarning("Rol no encontrado para eliminar: {RoleId}", id);
                    return NotFound("Rol no encontrado");
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar rol con ID: {RoleId}", id);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "Error interno del servidor" });
            }
        }
    }
}
