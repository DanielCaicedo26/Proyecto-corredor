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
        /// Obtiene todas las relaciones usuario-rol
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(List<UserRoleDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<UserRoleDto>>> GetAll()
        {
            try
            {
                _logger.LogInformation("Obteniendo todas las relaciones usuario-rol");
                var userRoles = await _userRoleService.GetAllAsync();
                return Ok(userRoles);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener relaciones usuario-rol");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "Error interno del servidor" });
            }
        }

        /// <summary>
        /// Obtiene una relación usuario-rol por ID
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(UserRoleDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<UserRoleDto>> GetById(int id)
        {
            try
            {
                if (id <= 0)
                    return BadRequest("ID debe ser mayor a 0");

                _logger.LogInformation("Obteniendo relación usuario-rol con ID: {Id}", id);
                var userRole = await _userRoleService.GetByIdAsync(id);

                if (userRole == null)
                {
                    _logger.LogWarning("Relación usuario-rol no encontrada: {Id}", id);
                    return NotFound("Relación usuario-rol no encontrada");
                }

                return Ok(userRole);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener relación usuario-rol por ID: {Id}", id);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "Error interno del servidor" });
            }
        }

        /// <summary>
        /// Crea una nueva relación usuario-rol
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(UserRoleDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<UserRoleDto>> Create([FromBody] UserRoleDto userRoleDto)
        {
            try
            {
                if (userRoleDto == null)
                    return BadRequest("Los datos de la relación usuario-rol son requeridos");

                _logger.LogInformation("Creando nueva relación usuario-rol");
                var createdUserRole = await _userRoleService.CreateAsync(userRoleDto);
                return CreatedAtAction(nameof(GetById), new { id = createdUserRole.Id }, createdUserRole);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear relación usuario-rol");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "Error interno del servidor" });
            }
        }

        /// <summary>
        /// Actualiza una relación usuario-rol
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(UserRoleDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<UserRoleDto>> Update(int id, [FromBody] UserRoleDto userRoleDto)
        {
            try
            {
                if (id <= 0)
                    return BadRequest("ID debe ser mayor a 0");

                if (userRoleDto == null)
                    return BadRequest("Los datos de la relación usuario-rol son requeridos");

                userRoleDto.Id = id;
                _logger.LogInformation("Actualizando relación usuario-rol con ID: {Id}", id);
                var updatedUserRole = await _userRoleService.UpdateAsync(userRoleDto);

                if (updatedUserRole == null)
                {
                    _logger.LogWarning("Relación usuario-rol no encontrada para actualizar: {Id}", id);
                    return NotFound("Relación usuario-rol no encontrada");
                }

                return Ok(updatedUserRole);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar relación usuario-rol con ID: {Id}", id);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "Error interno del servidor" });
            }
        }

        /// <summary>
        /// Elimina una relación usuario-rol
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

                _logger.LogInformation("Eliminando relación usuario-rol con ID: {Id}", id);
                var result = await _userRoleService.DeleteAsync(id);

                if (!result)
                {
                    _logger.LogWarning("Relación usuario-rol no encontrada para eliminar: {Id}", id);
                    return NotFound("Relación usuario-rol no encontrada");
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar relación usuario-rol con ID: {Id}", id);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "Error interno del servidor" });
            }
        }
    }
}
