# 📚 Guía Completa: Capa Data (Repositorios)

## ¿Qué es la Capa Data?

La **capa Data** es donde guardamos **TODOS** los accesos a la base de datos. Es como un intermediario entre tu Entity Framework y el resto de la aplicación.

```
┌─────────────────────────────────────────────────┐
│         Controlador / Servicio                   │
│         (Pide datos aquí)                        │
└────────────────┬────────────────────────────────┘
                 │
┌────────────────▼────────────────────────────────┐
│          CAPA DATA (Repositorios)                │
│     ◄─── Aquí hacemos consultas a BD            │
│          - IGenericRepository                    │
│          - IUserRepository                       │
│          - IRoleRepository                       │
│          - etc...                                │
└────────────────┬────────────────────────────────┘
                 │
┌────────────────▼────────────────────────────────┐
│      Entity Framework + SQL Server               │
│      (La base de datos)                          │
└─────────────────────────────────────────────────┘
```

---

## Estructura de la Carpeta Data

```
Data/
├── Interfaces/
│   ├── IGenericRepository.cs          ◄─── Base para todos
│   ├── IUserRepository.cs             ◄─── Específico de User
│   ├── IRoleRepository.cs             ◄─── Específico de Role
│   └── ... otros repositorios
│
├── Repositories/
│   ├── base/
│   │   └── GenericRepository.cs       ◄─── Implementación base
│   ├── UserRepository.cs              ◄─── Implementa IUserRepository
│   ├── RoleRepository.cs              ◄─── Implementa IRoleRepository
│   └── ... otros repositorios
│
└── Mappings/
    └── MappingProfile.cs              ◄─── Configuración de AutoMapper
```

---

## Paso 1: Entender IGenericRepository

### ¿Qué es?
Una **interfaz genérica** que define las operaciones CRUD básicas que TODOS los repositorios comparten.

### ¿Dónde está?
`Data/Interfaces/base/IGenericRepository.cs`

### ¿Qué contiene?

```csharp
namespace Data.Interfaces
{
    // Esta es la interfaz GENÉRICA
    // Funciona para CUALQUIER DTO (UserDto, RoleDto, etc.)
    public interface IGenericRepository<TDto> where TDto : class
    {
        // OPERACIONES BÁSICAS (CRUD)
        
        // Obtener por ID
        Task<TDto> GetByIdAsync(int id);
        
        // Obtener todos
        Task<List<TDto>> GetAllAsync();
        
        // Crear
        Task<TDto> AddAsync(TDto dto);
        
        // Actualizar
        Task<TDto> UpdateAsync(TDto dto);
        
        // Eliminar
        Task<bool> DeleteAsync(int id);
        
        // Verificar si existe
        Task<bool> ExistsAsync(int id);
    }
}
```

**En palabras simples:**
- `GetByIdAsync(5)` → Trae el Usuario con ID 5
- `GetAllAsync()` → Trae TODOS los usuarios
- `AddAsync(dto)` → Crea un nuevo usuario
- `UpdateAsync(dto)` → Actualiza un usuario
- `DeleteAsync(5)` → Elimina el usuario con ID 5

---

## Paso 2: Crear Interfaz Específica (IUserRepository)

### ¿Cuándo necesito esto?
Cuando quiero operaciones **especiales** que NO son CRUD básico.

Ejemplo: "Dame todos los usuarios por rol" o "Dame usuario por email"

### ¿Dónde lo pongo?
`Data/Interfaces/IUserRepository.cs`

### Código:

```csharp
using Entity.Dtos;

namespace Data.Interfaces
{
    // Hereda de IGenericRepository
    // Eso significa que AUTOMÁTICAMENTE tiene GetByIdAsync, GetAllAsync, etc.
    public interface IUserRepository : IGenericRepository<UserDto>
    {
        // Operaciones ESPECIALES de User (no CRUD básico)
        
        // Obtener usuario por nombre de usuario
        Task<UserDto> GetByUsernameAsync(string username);
        
        // Obtener usuario por email
        Task<UserDto> GetByEmailAsync(string email);
        
        // Obtener todos los usuarios de un rol
        Task<List<UserDto>> GetUsersByRoleAsync(int roleId);
    }
}
```

**¿Qué heredamos?**
```
IUserRepository
├─ GetByIdAsync(id)          ◄─ De IGenericRepository
├─ GetAllAsync()              ◄─ De IGenericRepository
├─ AddAsync(dto)              ◄─ De IGenericRepository
├─ UpdateAsync(dto)           ◄─ De IGenericRepository
├─ DeleteAsync(id)            ◄─ De IGenericRepository
├─ ExistsAsync(id)            ◄─ De IGenericRepository
├─ GetByUsernameAsync(...)    ◄─ NUEVA (específica)
├─ GetByEmailAsync(...)       ◄─ NUEVA (específica)
└─ GetUsersByRoleAsync(...)   ◄─ NUEVA (específica)
```

---

## Paso 3: Implementar GenericRepository (Base)

### ¿Qué es?
Una clase que **IMPLEMENTA** IGenericRepository con la lógica CRUD estándar.

### ¿Dónde lo pongo?
`Data/Repositories/base/GenericRepository.cs`

### Código:

```csharp
using Entity.DBcontext;
using Entity.Dtos;
using Entity.Entities;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

namespace Data.Repositories
{
    // Esta clase es GENÉRICA (funciona para cualquier Entity y DTO)
    public class GenericRepository<TEntity, TDto> : IGenericRepository<TDto>
        where TEntity : class, IEntity  // TEntity debe ser una Entity
        where TDto : class              // TDto debe ser un DTO
    {
        // Protegido = disponible en clases hijas
        protected readonly ApplicationDbContext _context;
        protected readonly DbSet<TEntity> _dbSet;
        protected readonly IMapper _mapper;

        public GenericRepository(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
            _mapper = mapper;
        }

        // IMPLEMENTACIÓN DE GetByIdAsync
        public async Task<TDto> GetByIdAsync(int id)
        {
            // 1. Buscar en BD
            var entity = await _dbSet
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.Id == id);
            
            // 2. Si no existe, devolver null
            if (entity == null)
                return null;
            
            // 3. Convertir Entity a DTO con AutoMapper
            return _mapper.Map<TDto>(entity);
        }

        // IMPLEMENTACIÓN DE GetAllAsync
        public async Task<List<TDto>> GetAllAsync()
        {
            // 1. Traer todos de BD
            var entities = await _dbSet
                .AsNoTracking()
                .ToListAsync();
            
            // 2. Convertir a DTOs
            return _mapper.Map<List<TDto>>(entities);
        }

        // IMPLEMENTACIÓN DE AddAsync
        public async Task<TDto> AddAsync(TDto dto)
        {
            // 1. Convertir DTO a Entity
            var entity = _mapper.Map<TEntity>(dto);
            
            // 2. Agregar a BD
            await _dbSet.AddAsync(entity);
            
            // 3. Guardar cambios
            await _context.SaveChangesAsync();
            
            // 4. Convertir de vuelta a DTO
            return _mapper.Map<TDto>(entity);
        }

        // IMPLEMENTACIÓN DE UpdateAsync
        public async Task<TDto> UpdateAsync(TDto dto)
        {
            // 1. Convertir DTO a Entity
            var entity = _mapper.Map<TEntity>(dto);
            
            // 2. Marcar como modificado
            _dbSet.Update(entity);
            
            // 3. Guardar cambios
            await _context.SaveChangesAsync();
            
            // 4. Devolver DTO
            return _mapper.Map<TDto>(entity);
        }

        // IMPLEMENTACIÓN DE DeleteAsync
        public async Task<bool> DeleteAsync(int id)
        {
            // 1. Buscar la entidad
            var entity = await _dbSet.FirstOrDefaultAsync(e => e.Id == id);
            
            // 2. Si no existe, devolver false
            if (entity == null)
                return false;
            
            // 3. Eliminar
            _dbSet.Remove(entity);
            
            // 4. Guardar cambios
            await _context.SaveChangesAsync();
            
            return true;
        }

        // IMPLEMENTACIÓN DE ExistsAsync
        public async Task<bool> ExistsAsync(int id)
        {
            return await _dbSet.AnyAsync(e => e.Id == id);
        }
    }
}
```

### ¿Qué hace cada método?

| Método | ¿Qué hace? |
|--------|-----------|
| `GetByIdAsync(5)` | `SELECT * FROM Users WHERE Id = 5` |
| `GetAllAsync()` | `SELECT * FROM Users` |
| `AddAsync(dto)` | `INSERT INTO Users (...)` |
| `UpdateAsync(dto)` | `UPDATE Users SET ...` |
| `DeleteAsync(5)` | `DELETE FROM Users WHERE Id = 5` |
| `ExistsAsync(5)` | `SELECT EXISTS(SELECT 1 FROM Users WHERE Id = 5)` |

---

## Paso 4: Implementar UserRepository

### ¿Qué es?
Una clase que **IMPLEMENTA** IUserRepository y hereda de GenericRepository.

### ¿Dónde lo pongo?
`Data/Repositories/UserRepository.cs`

### Código:

```csharp
using Entity.DBcontext;
using Entity.Dtos;
using Entity.Entities;
using Microsoft.EntityFrameworkCore;
using Data.Interfaces;
using AutoMapper;

namespace Data.Repositories
{
    public class UserRepository : GenericRepository<User, UserDto>, IUserRepository
    {
        // El constructor recibe el contexto y el mapper
        public UserRepository(ApplicationDbContext context, IMapper mapper) 
            : base(context, mapper)
        {
            // No necesitamos más código aquí
            // Ya tenemos GetByIdAsync, GetAllAsync, etc. de la clase base
        }

        // IMPLEMENTAR GetByUsernameAsync
        public async Task<UserDto> GetByUsernameAsync(string username)
        {
            // 1. Buscar usuario por username
            var user = await _dbSet
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Username == username);
            
            // 2. Convertir a DTO (o null si no existe)
            return _mapper.Map<UserDto>(user);
        }

        // IMPLEMENTAR GetByEmailAsync
        public async Task<UserDto> GetByEmailAsync(string email)
        {
            // 1. Buscar usuario por email
            var user = await _dbSet
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Email == email);
            
            // 2. Convertir a DTO
            return _mapper.Map<UserDto>(user);
        }

        // IMPLEMENTAR GetUsersByRoleAsync
        public async Task<List<UserDto>> GetUsersByRoleAsync(int roleId)
        {
            // 1. Buscar usuarios que tengan este rol
            // El _ indica que no nos importa el nombre de la variable
            var users = await _dbSet
                .AsNoTracking()
                .Where(u => u.UserRoles.Any(ur => ur.RoleId == roleId))
                .ToListAsync();
            
            // 2. Convertir todos a DTOs
            return _mapper.Map<List<UserDto>>(users);
        }
    }
}
```

### ¿De dónde vienen GetByIdAsync, GetAllAsync, etc.?

**De la clase base GenericRepository:**

```
UserRepository
│
├─ Hereda de GenericRepository<User, UserDto>
│  │
│  └─ GenericRepository implementa IGenericRepository<UserDto>
│     ├─ GetByIdAsync()      ◄─ Ya está implementado
│     ├─ GetAllAsync()        ◄─ Ya está implementado
│     ├─ AddAsync()           ◄─ Ya está implementado
│     ├─ UpdateAsync()        ◄─ Ya está implementado
│     ├─ DeleteAsync()        ◄─ Ya está implementado
│     └─ ExistsAsync()        ◄─ Ya está implementado
│
└─ Implementa IUserRepository
   ├─ GetByUsernameAsync()    ◄─ Nuevo (lo escribimos)
   ├─ GetByEmailAsync()       ◄─ Nuevo (lo escribimos)
   └─ GetUsersByRoleAsync()   ◄─ Nuevo (lo escribimos)
```

---

## Paso 5: Registrar en Program.cs

### ¿Por qué?
Para que cuando la aplicación necesite un `IUserRepository`, sepa que use `UserRepository`.

### ¿Dónde?
`Modelo de security/Program.cs`

### Código:

```csharp
// En la sección de servicios (antes de app.Build())

// Primero: DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// Segundo: AutoMapper
builder.Services.AddAutoMapper(typeof(MappingProfile));

// Tercero: Los Repositorios
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IPersonaRepository, PersonaRepository>();
// ... etc para todos los repositorios
```

### ¿Qué significa AddScoped?

```
┌─────────────────────────────────────────────┐
│           HTTP Request 1                     │
│  ┌──────────────────────────────────────┐   │
│  │  UserRepository (Instancia 1)        │   │
│  │  - Vive solo durante este request    │   │
│  └──────────────────────────────────────┘   │
└─────────────────────────────────────────────┘

┌─────────────────────────────────────────────┐
│           HTTP Request 2                     │
│  ┌──────────────────────────────────────┐   │
│  │  UserRepository (Instancia 2)        │   │
│  │  - Es NUEVA (diferente a la anterior)│   │
│  └──────────────────────────────────────┘   │
└─────────────────────────────────────────────┘
```

**Scoped = Una nueva instancia por cada solicitud HTTP**

---

## Paso 6: Usar el Repositorio en un Servicio

### ¿Dónde?
`Bussines/Services/UserService.cs`

### Código:

```csharp
using Entity.Dtos;
using Data.Interfaces;

namespace Bussines.Services
{
    public class UserService : IUserService
    {
        // 1. Inyectar el repositorio
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        // 2. Usar el repositorio
        public async Task<UserDto> GetByIdAsync(int id)
        {
            // Llama al repositorio que llama a la BD
            return await _userRepository.GetByIdAsync(id);
        }

        public async Task<List<UserDto>> GetAllAsync()
        {
            return await _userRepository.GetAllAsync();
        }

        public async Task<UserDto> GetByEmailAsync(string email)
        {
            // Aquí usamos el método ESPECIAL que escribimos
            return await _userRepository.GetByEmailAsync(email);
        }

        public async Task<bool> CreateUserAsync(UserDto dto)
        {
            // Validaciones de negocio
            if (string.IsNullOrWhiteSpace(dto.Email))
                throw new ArgumentException("Email es requerido");

            // Verificar que el email no existe
            var existing = await _userRepository.GetByEmailAsync(dto.Email);
            if (existing != null)
                throw new InvalidOperationException("Este email ya está registrado");

            // Si todo está bien, agregar a BD
            await _userRepository.AddAsync(dto);
            return true;
        }
    }
}
```

### Flujo completo:

```
Controlador (API)
    │
    ▼
┌─────────────────┐
│  UserService    │  ◄─ "Dame todos los usuarios"
└────────┬────────┘
         │
         ▼
┌──────────────────────┐
│  UserRepository      │  ◄─ "OK, voy a consultar la BD"
│  (GetAllAsync)       │
└────────┬─────────────┘
         │
         ▼
┌──────────────────────┐
│  Entity Framework    │  ◄─ SELECT * FROM Users
│  + SQL Server        │
└─────────────────────┘
         │
         ▼ (Resultados)
┌──────────────────────┐
│  GenericRepository   │  ◄─ Convertir de Entity a DTO
│  (usando _mapper)    │
└─────────────────────┘
         │
         ▼ (DTOs)
┌──────────────────────┐
│  UserService         │  ◄─ Devolver al controlador
└──────────────────────┘
         │
         ▼
┌──────────────────────┐
│  Controlador         │  ◄─ Devolver al cliente (JSON)
└──────────────────────┘
```

---

## Paso 7: AutoMapper - ¿Cómo convierte Entity a DTO?

### ¿Dónde?
`Data/Mappings/MappingProfile.cs`

### Código:

```csharp
using AutoMapper;
using Entity.Dtos;
using Entity.Entities;

namespace Data.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Decir a AutoMapper cómo convertir User a UserDto
            CreateMap<User, UserDto>()
                // Caso especial: User.Created → UserDto.RegistrationDate
                .ForMember(dest => dest.RegistrationDate, 
                           opt => opt.MapFrom(src => src.Created))
                .ReverseMap();  // También permite convertir de DTO a Entity
            
            // Lo mismo para Role
            CreateMap<Role, RoleDto>().ReverseMap();
            
            // Y así para todas las entidades...
            CreateMap<Persona, PersonaDto>().ReverseMap();
        }
    }
}
```

### ¿Cómo funciona?

**Sin AutoMapper (manual):**
```csharp
// ¡Hay que hacer esto manualmente!
var userDto = new UserDto
{
    Id = user.Id,
    Username = user.Username,
    Email = user.Email,
    RegistrationDate = user.Created  // Nota: Created → RegistrationDate
};
```

**Con AutoMapper:**
```csharp
// ¡AutoMapper lo hace automáticamente!
var userDto = _mapper.Map<UserDto>(user);
```

---

## Resumen: Pasos para crear un Repositorio

### 1️⃣ Crear Interfaz
**Archivo:** `Data/Interfaces/I[Entidad]Repository.cs`
```csharp
public interface IUserRepository : IGenericRepository<UserDto>
{
    Task<UserDto> GetByEmailAsync(string email);
}
```

### 2️⃣ Crear Implementación
**Archivo:** `Data/Repositories/[Entidad]Repository.cs`
```csharp
public class UserRepository : GenericRepository<User, UserDto>, IUserRepository
{
    public UserRepository(ApplicationDbContext context, IMapper mapper) 
        : base(context, mapper) { }

    public async Task<UserDto> GetByEmailAsync(string email)
    {
        var user = await _dbSet
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Email == email);
        return _mapper.Map<UserDto>(user);
    }
}
```

### 3️⃣ Registrar en Program.cs
```csharp
builder.Services.AddScoped<IUserRepository, UserRepository>();
```

### 4️⃣ Usar en Servicio
```csharp
public class UserService
{
    private readonly IUserRepository _repo;
    public UserService(IUserRepository repo) => _repo = repo;
    
    public async Task<UserDto> GetByEmailAsync(string email)
        => await _repo.GetByEmailAsync(email);
}
```

### 5️⃣ Usar en Controlador
```csharp
public class UsersController
{
    private readonly IUserService _service;
    public UsersController(IUserService service) => _service = service;
    
    [HttpGet("email")]
    public async Task<ActionResult> GetByEmail(string email)
        => Ok(await _service.GetByEmailAsync(email));
}
```

---

## ¿Cuándo escribir métodos especiales?

| Caso | ¿Necesito método especial? | Ejemplo |
|------|----------------------------|---------|
| Obtener por ID | NO | GetByIdAsync ya existe |
| Obtener todos | NO | GetAllAsync ya existe |
| Crear uno | NO | AddAsync ya existe |
| Actualizar | NO | UpdateAsync ya existe |
| Eliminar | NO | DeleteAsync ya existe |
| **Obtener por email** | **SÍ** | GetByEmailAsync |
| **Obtener por rol** | **SÍ** | GetUsersByRoleAsync |
| **Buscar por rango de fechas** | **SÍ** | GetByDateRangeAsync |
| **Filtrar por estado** | **SÍ** | GetByStatusAsync |

---

## Errores Comunes y Soluciones

### Error 1: "¿Dónde escribo la consulta SQL?"
**Respuesta:** No escribes SQL. Entity Framework lo hace por ti.

```csharp
// ✅ CORRECTO
var users = await _dbSet
    .Where(u => u.Email == email)
    .FirstOrDefaultAsync();

// ❌ INCORRECTO (no hacer esto)
var users = await _context.Database.ExecuteSqlAsync("SELECT ...");
```

### Error 2: "¿Dónde va AsNoTracking()?"
**Respuesta:** Cuando SOLO LEES, no modificas.

```csharp
// ✅ LEER - usar AsNoTracking()
var user = await _dbSet.AsNoTracking()
    .FirstOrDefaultAsync(u => u.Id == id);

// ✅ MODIFICAR - NO usar AsNoTracking()
var user = await _dbSet.FirstOrDefaultAsync(u => u.Id == id);
user.Email = "nuevo@email.com";
_dbSet.Update(user);
```

### Error 3: "_mapper no está definido"
**Respuesta:** Necesitas inyectarlo en el constructor.

```csharp
// ✅ CORRECTO
public UserRepository(ApplicationDbContext context, IMapper mapper) 
    : base(context, mapper)
{
}

// ❌ INCORRECTO
public UserRepository(ApplicationDbContext context) 
    : base(context, null)  // Falta IMapper
{
}
```

---

¡Ahora entiendes cómo funciona la capa Data! 🎉
