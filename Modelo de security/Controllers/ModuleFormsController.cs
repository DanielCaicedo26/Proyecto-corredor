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
    /// Controller para gestionar Formas de Módulos
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class ModuleFormsController : ControllerBase
    {
        private readonly IModuleFormService _moduleFormService;
        private readonly ILogger<ModuleFormsController> _logger;

        public ModuleFormsController(IModuleFormService moduleFormService, ILogger<ModuleFormsController> logger)
        {
            _moduleFormService = moduleFormService ?? throw new ArgumentNullException(nameof(moduleFormService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Obtiene todas las formas de módulos
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(List<ModuleFormDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<ModuleFormDto>>> GetAll()
        {
            try
            {
                _logger.LogInformation("Obteniendo todas las formas de módulos");
                var moduleForms = await _moduleFormService.GetAllAsync();
                return Ok(moduleForms);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todas las formas de módulos");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "Error interno del servidor" });
            }
        }

        /// <summary>
        /// Obtiene una forma de módulo por ID
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ModuleFormDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ModuleFormDto>> GetById(int id)
        {
            try
            {
                if (id <= 0)
                    return BadRequest("ID debe ser mayor a 0");

                _logger.LogInformation("Obteniendo forma de módulo con ID: {ModuleFormId}", id);
                var moduleForm = await _moduleFormService.GetByIdAsync(id);

                if (moduleForm == null)
                {
                    _logger.LogWarning("Forma de módulo no encontrada: {ModuleFormId}", id);
                    return NotFound("Forma de módulo no encontrada");
                }

                return Ok(moduleForm);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener forma de módulo por ID: {ModuleFormId}", id);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "Error interno del servidor" });
            }
        }

        /// <summary>
        /// Obtiene formas de un módulo
        /// </summary>
        [HttpGet("by-modulo/{moduloId}")]
        [ProducesResponseType(typeof(List<ModuleFormDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<ModuleFormDto>>> GetModuleFormsByModulo(int moduloId)
        {
            try
            {
                if (moduloId <= 0)
                    return BadRequest("ID de módulo debe ser mayor a 0");

                _logger.LogInformation("Obteniendo formas del módulo: {ModuloId}", moduloId);
                var moduleForms = await _moduleFormService.GetModuleFormsByModuloAsync(moduloId);
                return Ok(moduleForms);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener formas del módulo: {ModuloId}", moduloId);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "Error interno del servidor" });
            }
        }

        /// <summary>
        /// Obtiene módulos de una forma
        /// </summary>
        [HttpGet("by-forma/{formaId}")]
        [ProducesResponseType(typeof(List<ModuleFormDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<ModuleFormDto>>> GetModuleFormsByForma(int formaId)
        {
            try
            {
                if (formaId <= 0)
                    return BadRequest("ID de forma debe ser mayor a 0");

                _logger.LogInformation("Obteniendo módulos de la forma: {FormaId}", formaId);
                var moduleForms = await _moduleFormService.GetModuleFormsByFormaAsync(formaId);
                return Ok(moduleForms);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener módulos de la forma: {FormaId}", formaId);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "Error interno del servidor" });
            }
        }

        /// <summary>
        /// Obtiene la relación forma-módulo por IDs
        /// </summary>
        [HttpGet("by-ids")]
        [ProducesResponseType(typeof(ModuleFormDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ModuleFormDto>> GetByModuloAndForma([FromQuery] int moduloId, [FromQuery] int formaId)
        {
            try
            {
                if (moduloId <= 0 || formaId <= 0)
                    return BadRequest("IDs de módulo y forma deben ser mayores a 0");

                _logger.LogInformation("Obteniendo forma-módulo: ModuloId={ModuloId}, FormaId={FormaId}", moduloId, formaId);
                var moduleForm = await _moduleFormService.GetByModuloAndFormaAsync(moduloId, formaId);

                if (moduleForm == null)
                {
                    _logger.LogWarning("Forma-módulo no encontrada: ModuloId={ModuloId}, FormaId={FormaId}", moduloId, formaId);
                    return NotFound("Forma-módulo no encontrada");
                }

                return Ok(moduleForm);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener forma-módulo");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "Error interno del servidor" });
            }
        }

        /// <summary>
        /// Crea una nueva relación forma-módulo
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(ModuleFormDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ModuleFormDto>> Create([FromBody] ModuleFormDto moduleFormDto)
        {
            try
            {
                if (moduleFormDto == null)
                    return BadRequest("Los datos de la forma-módulo son requeridos");

                _logger.LogInformation("Creando nueva relación forma-módulo");
                var createdModuleForm = await _moduleFormService.CreateAsync(moduleFormDto);
                return CreatedAtAction(nameof(GetById), new { id = createdModuleForm.Id }, createdModuleForm);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear forma-módulo");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "Error interno del servidor" });
            }
        }

        /// <summary>
        /// Actualiza una relación forma-módulo
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ModuleFormDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ModuleFormDto>> Update(int id, [FromBody] ModuleFormDto moduleFormDto)
        {
            try
            {
                if (id <= 0)
                    return BadRequest("ID debe ser mayor a 0");

                if (moduleFormDto == null)
                    return BadRequest("Los datos de la forma-módulo son requeridos");

                moduleFormDto.Id = id;
                _logger.LogInformation("Actualizando forma-módulo con ID: {ModuleFormId}", id);
                var updatedModuleForm = await _moduleFormService.UpdateAsync(moduleFormDto);
                
                if (updatedModuleForm == null)
                {
                    _logger.LogWarning("Forma-módulo no encontrada para actualizar: {ModuleFormId}", id);
                    return NotFound("Forma-módulo no encontrada");
                }

                return Ok(updatedModuleForm);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar forma-módulo con ID: {ModuleFormId}", id);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "Error interno del servidor" });
            }
        }

        /// <summary>
        /// Elimina una relación forma-módulo
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

                _logger.LogInformation("Eliminando forma-módulo con ID: {ModuleFormId}", id);
                var result = await _moduleFormService.DeleteAsync(id);

                if (!result)
                {
                    _logger.LogWarning("Forma-módulo no encontrada para eliminar: {ModuleFormId}", id);
                    return NotFound("Forma-módulo no encontrada");
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar forma-módulo con ID: {ModuleFormId}", id);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "Error interno del servidor" });
            }
        }
    }
}
