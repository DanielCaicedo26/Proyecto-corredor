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
    /// Controller para gestionar Personas
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class PersonasController : ControllerBase
    {
        private readonly IPersonaService _personaService;
        private readonly ILogger<PersonasController> _logger;

        public PersonasController(IPersonaService personaService, ILogger<PersonasController> logger)
        {
            _personaService = personaService ?? throw new ArgumentNullException(nameof(personaService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Obtiene todas las personas
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(List<PersonaDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<PersonaDto>>> GetAll()
        {
            try
            {
                _logger.LogInformation("Obteniendo todas las personas");
                var personas = await _personaService.GetAllAsync();
                return Ok(personas);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todas las personas");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "Error interno del servidor" });
            }
        }

        /// <summary>
        /// Obtiene una persona por ID
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(PersonaDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PersonaDto>> GetById(int id)
        {
            try
            {
                if (id <= 0)
                    return BadRequest("ID debe ser mayor a 0");

                _logger.LogInformation("Obteniendo persona con ID: {PersonaId}", id);
                var persona = await _personaService.GetByIdAsync(id);

                if (persona == null)
                {
                    _logger.LogWarning("Persona no encontrada: {PersonaId}", id);
                    return NotFound("Persona no encontrada");
                }

                return Ok(persona);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener persona por ID: {PersonaId}", id);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "Error interno del servidor" });
            }
        }

        /// <summary>
        /// Obtiene una persona por número de documento
        /// </summary>
        [HttpGet("by-document/{documentNumber}")]
        [ProducesResponseType(typeof(PersonaDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PersonaDto>> GetByDocumentNumber(string documentNumber)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(documentNumber))
                    return BadRequest("El número de documento es requerido");

                _logger.LogInformation("Obteniendo persona con documento: {DocumentNumber}", documentNumber);
                var persona = await _personaService.GetByDocumentNumberAsync(documentNumber);

                if (persona == null)
                {
                    _logger.LogWarning("Persona no encontrada con documento: {DocumentNumber}", documentNumber);
                    return NotFound("Persona no encontrada");
                }

                return Ok(persona);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener persona por documento: {DocumentNumber}", documentNumber);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "Error interno del servidor" });
            }
        }

        /// <summary>
        /// Obtiene personas con usuarios asociados
        /// </summary>
        [HttpGet("with-users")]
        [ProducesResponseType(typeof(List<PersonaDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<PersonaDto>>> GetPersonasWithUsers()
        {
            try
            {
                _logger.LogInformation("Obteniendo personas con usuarios");
                var personas = await _personaService.GetPersonasWithUsersAsync();
                return Ok(personas);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener personas con usuarios");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "Error interno del servidor" });
            }
        }

        /// <summary>
        /// Crea una nueva persona
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(PersonaDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PersonaDto>> Create([FromBody] PersonaDto personaDto)
        {
            try
            {
                if (personaDto == null)
                    return BadRequest("Los datos de la persona son requeridos");

                if (string.IsNullOrWhiteSpace(personaDto.Name) || string.IsNullOrWhiteSpace(personaDto.LastName))
                    return BadRequest("El nombre y apellido son requeridos");

                _logger.LogInformation("Creando nueva persona: {PersonaName}", personaDto.Name);
                var createdPersona = await _personaService.CreateAsync(personaDto);
                return CreatedAtAction(nameof(GetById), new { id = createdPersona.Id }, createdPersona);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear persona");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "Error interno del servidor" });
            }
        }

        /// <summary>
        /// Actualiza una persona existente
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(PersonaDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PersonaDto>> Update(int id, [FromBody] PersonaDto personaDto)
        {
            try
            {
                if (id <= 0)
                    return BadRequest("ID debe ser mayor a 0");

                if (personaDto == null)
                    return BadRequest("Los datos de la persona son requeridos");

                personaDto.Id = id;
                _logger.LogInformation("Actualizando persona con ID: {PersonaId}", id);
                var updatedPersona = await _personaService.UpdateAsync(personaDto);
                
                if (updatedPersona == null)
                {
                    _logger.LogWarning("Persona no encontrada para actualizar: {PersonaId}", id);
                    return NotFound("Persona no encontrada");
                }

                return Ok(updatedPersona);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar persona con ID: {PersonaId}", id);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "Error interno del servidor" });
            }
        }

        /// <summary>
        /// Elimina una persona
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

                _logger.LogInformation("Eliminando persona con ID: {PersonaId}", id);
                var result = await _personaService.DeleteAsync(id);

                if (!result)
                {
                    _logger.LogWarning("Persona no encontrada para eliminar: {PersonaId}", id);
                    return NotFound("Persona no encontrada");
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar persona con ID: {PersonaId}", id);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "Error interno del servidor" });
            }
        }
    }
}
