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
    /// Controller para gestionar Formas
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class FormasController : ControllerBase
    {
        private readonly IFormaService _formaService;
        private readonly ILogger<FormasController> _logger;

        public FormasController(IFormaService formaService, ILogger<FormasController> logger)
        {
            _formaService = formaService ?? throw new ArgumentNullException(nameof(formaService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Obtiene todas las formas
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(List<FormaDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<FormaDto>>> GetAll()
        {
            try
            {
                _logger.LogInformation("Obteniendo todas las formas");
                var formas = await _formaService.GetAllAsync();
                return Ok(formas);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todas las formas");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "Error interno del servidor" });
            }
        }

        /// <summary>
        /// Obtiene una forma por ID
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(FormaDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<FormaDto>> GetById(int id)
        {
            try
            {
                if (id <= 0)
                    return BadRequest("ID debe ser mayor a 0");

                _logger.LogInformation("Obteniendo forma con ID: {FormaId}", id);
                var forma = await _formaService.GetByIdAsync(id);

                if (forma == null)
                {
                    _logger.LogWarning("Forma no encontrada: {FormaId}", id);
                    return NotFound("Forma no encontrada");
                }

                return Ok(forma);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener forma por ID: {FormaId}", id);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "Error interno del servidor" });
            }
        }

        /// <summary>
        /// Crea una nueva forma
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(FormaDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<FormaDto>> Create([FromBody] FormaDto formaDto)
        {
            try
            {
                if (formaDto == null)
                    return BadRequest("Los datos de la forma son requeridos");

                if (string.IsNullOrWhiteSpace(formaDto.Name))
                    return BadRequest("El nombre de la forma es requerido");

                _logger.LogInformation("Creando nueva forma: {FormaName}", formaDto.Name);
                var createdForma = await _formaService.CreateAsync(formaDto);
                return CreatedAtAction(nameof(GetById), new { id = createdForma.Id }, createdForma);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear forma");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "Error interno del servidor" });
            }
        }

        /// <summary>
        /// Actualiza una forma existente
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(FormaDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<FormaDto>> Update(int id, [FromBody] FormaDto formaDto)
        {
            try
            {
                if (id <= 0)
                    return BadRequest("ID debe ser mayor a 0");

                if (formaDto == null)
                    return BadRequest("Los datos de la forma son requeridos");

                formaDto.Id = id;
                _logger.LogInformation("Actualizando forma con ID: {FormaId}", id);
                var updatedForma = await _formaService.UpdateAsync(formaDto);
                
                if (updatedForma == null)
                {
                    _logger.LogWarning("Forma no encontrada para actualizar: {FormaId}", id);
                    return NotFound("Forma no encontrada");
                }

                return Ok(updatedForma);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar forma con ID: {FormaId}", id);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "Error interno del servidor" });
            }
        }

        /// <summary>
        /// Elimina una forma
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

                _logger.LogInformation("Eliminando forma con ID: {FormaId}", id);
                var result = await _formaService.DeleteAsync(id);

                if (!result)
                {
                    _logger.LogWarning("Forma no encontrada para eliminar: {FormaId}", id);
                    return NotFound("Forma no encontrada");
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar forma con ID: {FormaId}", id);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "Error interno del servidor" });
            }
        }
    }
}
