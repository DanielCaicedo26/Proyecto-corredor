# 🔐 Cómo Proteger tus Controladores con [Authorize]

## Introducción

Una vez que JWT esté configurado, puedes proteger tus endpoints usando el atributo `[Authorize]`.

---

## Tipos de Atributos

### 1. Proteger todo el controlador
```csharp
[Authorize]
[ApiController]
[Route("api/[controller]")]
public class RolesController : ControllerBase
{
    // Todos los métodos requieren token
    [HttpGet]
    public async Task<ActionResult<List<RoleDto>>> GetAll()
    {
        // ...
    }
}
```

### 2. Proteger solo ciertos métodos
```csharp
[ApiController]
[Route("api/[controller]")]
public class RolesController : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<RoleDto>>> GetAll()
    {
        // Público, sin token
    }

    [Authorize]  // ← Solo este método requiere token
    [HttpPost]
    public async Task<ActionResult<RoleDto>> Create([FromBody] RoleDto dto)
    {
        // Requiere token
    }
}
```

### 3. Proteger solo métodos específicos
```csharp
[ApiController]
[Route("api/[controller]")]
public class RolesController : ControllerBase
{
    [HttpGet("{id}")]
    public async Task<ActionResult<RoleDto>> GetById(int id)
    {
        // Público
    }

    [Authorize]
    [HttpPut("{id}")]
    public async Task<ActionResult<RoleDto>> Update(int id, [FromBody] RoleDto dto)
    {
        // Requiere token
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        // Requiere token
    }
}
```

---

## Autorización por Roles (Futuro)

Una vez que implementes roles en tu JWT, puedes hacer:

```csharp
[Authorize(Roles = "Admin")]
[HttpDelete("{id}")]
public async Task<IActionResult> Delete(int id)
{
    // Solo los usuarios con rol "Admin" pueden eliminar
}
```

---

## Obtener el Usuario Autenticado

Dentro de un método con `[Authorize]`, puedes obtener la información del usuario:

```csharp
[Authorize]
[HttpGet("me")]
public async Task<ActionResult<UserDto>> GetCurrentUser()
{
    var userId = User.FindFirst("sub")?.Value;
    var username = User.FindFirst(System.Security.Claims.ClaimTypes.Name)?.Value;
    
    _logger.LogInformation("Usuario actual: {Username}", username);
    
    return Ok(new { userId, username });
}
```

---

## Ejemplo: RolesController Protegido

```csharp
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Entity.Dtos;
using Bussines.Interfaces;

namespace Modelo_de_security.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RolesController : ControllerBase
    {
        private readonly IRoleService _roleService;
        private readonly ILogger<RolesController> _logger;

        public RolesController(IRoleService roleService, ILogger<RolesController> logger)
        {
            _roleService = roleService;
            _logger = logger;
        }

        /// <summary>
        /// Obtiene todos los roles (SIN autenticación)
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<List<RoleDto>>> GetAll()
        {
            try
            {
                var roles = await _roleService.GetAllAsync();
                return Ok(roles);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener roles");
                return StatusCode(500, new { error = "Error interno del servidor" });
            }
        }

        /// <summary>
        /// Obtiene un rol por ID (SIN autenticación)
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<RoleDto>> GetById(int id)
        {
            try
            {
                if (id <= 0)
                    return BadRequest(new { error = "ID debe ser mayor a 0" });

                var role = await _roleService.GetByIdAsync(id);
                return Ok(role);
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { error = "Rol no encontrado" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener rol");
                return StatusCode(500, new { error = "Error interno del servidor" });
            }
        }

        /// <summary>
        /// Crea un nuevo rol (Requiere autenticación)
        /// </summary>
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<RoleDto>> Create([FromBody] RoleDto dto)
        {
            try
            {
                if (dto == null)
                    return BadRequest(new { error = "Rol es requerido" });

                var createdRole = await _roleService.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = createdRole.Id }, createdRole);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear rol");
                return StatusCode(500, new { error = "Error interno del servidor" });
            }
        }

        /// <summary>
        /// Actualiza un rol (Requiere autenticación)
        /// </summary>
        [Authorize]
        [HttpPut("{id}")]
        public async Task<ActionResult<RoleDto>> Update(int id, [FromBody] RoleDto dto)
        {
            try
            {
                if (id <= 0)
                    return BadRequest(new { error = "ID debe ser mayor a 0" });

                dto.Id = id;
                var updatedRole = await _roleService.UpdateAsync(dto);
                return Ok(updatedRole);
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { error = "Rol no encontrado" });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar rol");
                return StatusCode(500, new { error = "Error interno del servidor" });
            }
        }

        /// <summary>
        /// Elimina un rol (Requiere autenticación)
        /// </summary>
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                if (id <= 0)
                    return BadRequest(new { error = "ID debe ser mayor a 0" });

                var result = await _roleService.DeleteAsync(id);
                if (!result)
                    return NotFound(new { error = "Rol no encontrado" });

                return Ok(new { message = "Rol eliminado exitosamente" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar rol");
                return StatusCode(500, new { error = "Error interno del servidor" });
            }
        }
    }
}
```

---

## Flujo de Autenticación en Postman

### 1. Login
```
POST /api/auth/login
```
Obtén el token y cópialo.

### 2. Usa el Token
```
Authorization: Bearer {token}
```

### 3. Accede a Endpoint Protegido
```
POST /api/roles
Header: Authorization: Bearer {token}
```

Si el token es válido → ✅ 200 OK
Si no hay token → ❌ 401 Unauthorized
Si el token expiró → ❌ 401 Unauthorized

---

## Tabla Rápida

| Endpoint | Público | Con [Authorize] |
|---|---|---|
| GET /api/roles | ✅ | ❌ |
| GET /api/roles/{id} | ✅ | ❌ |
| POST /api/roles | ❌ | ✅ |
| PUT /api/roles/{id} | ❌ | ✅ |
| DELETE /api/roles/{id} | ❌ | ✅ |

---

## ⚠️ Importante

- `[Authorize]` sin token → 401 Unauthorized
- El token se envía en el header: `Authorization: Bearer {token}`
- El token expira cada 60 minutos (configurable en appsettings.json)
- Después de expirar, el usuario debe hacer login de nuevo

---

## ¿Cómo Implementar en Otros Controladores?

Simplemente copia y adapta el patrón de `UsersController.cs`:

```csharp
// 1. Importar
using Microsoft.AspNetCore.Authorization;

// 2. Agregar [Authorize] a métodos sensibles
[Authorize]
[HttpPost]
public async Task<ActionResult<T>> Create([FromBody] T dto)
{
    // ...
}
```

¡Eso es todo! 🎉
