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
    /// Controller para gestionar Módulos
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class ModulosController : ControllerBase
    {
        private readonly IModuloService _moduloService;
        private readonly ILogger<ModulosController> _logger;

        public ModulosController(IModuloService moduloService, ILogger<ModulosController> logger)
        {
            _moduloService = moduloService ?? throw new ArgumentNullException(nameof(moduloService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Obtiene todos los módulos
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(List<ModuloDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<ModuloDto>>> GetAll()
        {
            try
            {
                _logger.LogInformation("Obteniendo todos los módulos");
                var modulos = await _moduloService.GetAllAsync();
                return Ok(modulos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los módulos");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "Error interno del servidor" });
            }
        }

        /// <summary>
        /// Obtiene un módulo por ID
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ModuloDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ModuloDto>> GetById(int id)
        {
            try
            {
                if (id <= 0)
                    return BadRequest("ID debe ser mayor a 0");

                _logger.LogInformation("Obteniendo módulo con ID: {ModuloId}", id);
                var modulo = await _moduloService.GetByIdAsync(id);

                if (modulo == null)
                {
                    _logger.LogWarning("Módulo no encontrado: {ModuloId}", id);
                    return NotFound("Módulo no encontrado");
                }

                return Ok(modulo);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener módulo por ID: {ModuloId}", id);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "Error interno del servidor" });
            }
        }

        /// <summary>
        /// Crea un nuevo módulo
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(ModuloDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ModuloDto>> Create([FromBody] ModuloDto moduloDto)
        {
            try
            {
                if (moduloDto == null)
                    return BadRequest("Los datos del módulo son requeridos");

                if (string.IsNullOrWhiteSpace(moduloDto.Name))
                    return BadRequest("El nombre del módulo es requerido");

                _logger.LogInformation("Creando nuevo módulo: {ModuloName}", moduloDto.Name);
                var createdModulo = await _moduloService.CreateAsync(moduloDto);
                return CreatedAtAction(nameof(GetById), new { id = createdModulo.Id }, createdModulo);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear módulo");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "Error interno del servidor" });
            }
        }

        /// <summary>
        /// Actualiza un módulo existente
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ModuloDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ModuloDto>> Update(int id, [FromBody] ModuloDto moduloDto)
        {
            try
            {
                if (id <= 0)
                    return BadRequest("ID debe ser mayor a 0");

                if (moduloDto == null)
                    return BadRequest("Los datos del módulo son requeridos");

                moduloDto.Id = id;
                _logger.LogInformation("Actualizando módulo con ID: {ModuloId}", id);
                var updatedModulo = await _moduloService.UpdateAsync(moduloDto);
                
                if (updatedModulo == null)
                {
                    _logger.LogWarning("Módulo no encontrado para actualizar: {ModuloId}", id);
                    return NotFound("Módulo no encontrado");
                }

                return Ok(updatedModulo);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar módulo con ID: {ModuloId}", id);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "Error interno del servidor" });
            }
        }

        /// <summary>
        /// Elimina un módulo
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

                _logger.LogInformation("Eliminando módulo con ID: {ModuloId}", id);
                var result = await _moduloService.DeleteAsync(id);

                if (!result)
                {
                    _logger.LogWarning("Módulo no encontrado para eliminar: {ModuloId}", id);
                    return NotFound("Módulo no encontrado");
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar módulo con ID: {ModuloId}", id);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "Error interno del servidor" });
            }
        }
    }
}
