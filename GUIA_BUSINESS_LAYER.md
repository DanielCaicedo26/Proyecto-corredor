# 📚 Guía Completa: Capa Business (Servicios) - Con GenericService

## ¿Qué es la Capa Business?

La **capa Business** es donde va **TODA la lógica de negocio**. Es el "cerebro" de tu aplicación.

```
┌──────────────────────────────────────┐
│      Controlador (API)                │
│      (Recibe solicitudes HTTP)        │
└────────────────┬─────────────────────┘
                 │
┌────────────────▼──────────────────────┐
│    CAPA BUSINESS (Servicios)           │
│  ◄─── LÓGICA DE NEGOCIO AQUÍ          │
│    - Validaciones                      │
│    - Reglas de negocio                 │
│    - Transformaciones                  │
│    - Orquestación                      │
└────────────────┬──────────────────────┘
                 │
┌────────────────▼──────────────────────┐
│      CAPA DATA (Repositorios)          │
│      (Acceso a base de datos)          │
└─────────────────────────────────────┘
```

---

## 🎯 Nueva Arquitectura: GenericService<T>

### ¿Qué es GenericService<T>?

**GenericService<T>** es una clase **abstracta base** que implementa el CRUD básico para TODOS los servicios. 

**ANTES (Sin GenericService):**
```
UserService: IUserService
├── GetByIdAsync() { ... }
├── GetAllAsync() { ... }
├── CreateAsync() { ... }
├── UpdateAsync() { ... }
├── DeleteAsync() { ... }
└── GetByEmailAsync() { ... }

RoleService: IRoleService
├── GetByIdAsync() { ... }     ❌ DUPLICADO
├── GetAllAsync() { ... }      ❌ DUPLICADO
├── CreateAsync() { ... }      ❌ DUPLICADO
├── UpdateAsync() { ... }      ❌ DUPLICADO
├── DeleteAsync() { ... }      ❌ DUPLICADO
└── GetByNameAsync() { ... }
```

**AHORA (Con GenericService):**
```
GenericService<TDto> : IGenericService<TDto>
├── GetByIdAsync() { ... }     ✅ CÓDIGO COMPARTIDO
├── GetAllAsync() { ... }      ✅ CÓDIGO COMPARTIDO
├── CreateAsync() { ... }      ✅ CÓDIGO COMPARTIDO
├── UpdateAsync() { ... }      ✅ CÓDIGO COMPARTIDO
├── DeleteAsync() { ... }      ✅ CÓDIGO COMPARTIDO
└── ValidateData() { virtual } ✅ HOOK PARA HERENCIA

    ↑
    │ Hereda (8 servicios)
    │
    ├── UserService: GenericService<UserDto>
    │   └── GetByEmailAsync() { ... } ✅ SOLO MÉTODOS ESPECIALES
    │
    └── RoleService: GenericService<RoleDto>
        └── GetByNameAsync() { ... } ✅ SOLO MÉTODOS ESPECIALES
```

---

## Paso 1: IGenericService<T> - La Interfaz Base

### ¿Dónde?
`Bussines/Interfaces/IGenericService.cs`

### Código:

```csharp
using Entity.Dtos;

namespace Bussines.Interfaces
{
    public interface IGenericService<TDto> where TDto : BaseDto
    {
        // OPERACIONES CRUD BÁSICAS (para todos los servicios)
        Task<TDto> GetByIdAsync(int id);
        Task<List<TDto>> GetAllAsync();
        Task<TDto> CreateAsync(TDto dto);
        Task<TDto> UpdateAsync(TDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
```

**¿Qué es la restricción `where TDto : BaseDto`?**
- Solo acepta DTOs que hereden de `BaseDto`
- Garantiza que todos tienen propiedades comunes (Id, etc.)

---

## Paso 2: GenericService<T> - La Clase Base

### ¿Dónde?
`Bussines/Services/GenericService.cs`

### Código Completo:

```csharp
using Entity.Dtos;
using Data.Interfaces;
using Bussines.Interfaces;

namespace Bussines.Services
{
    /// <summary>
    /// Clase abstracta base que implementa CRUD genérico para todos los servicios
    /// </summary>
    /// <typeparam name="TDto">El DTO del servicio (UserDto, RoleDto, etc.)</typeparam>
    public abstract class GenericService<TDto> : IGenericService<TDto> where TDto : BaseDto
    {
        // ✅ Repositorio genérico (inyectado por subclases)
        protected readonly IGenericRepository<TDto> _repository;

        public GenericService(IGenericRepository<TDto> repository)
        {
            _repository = repository;
        }

        // ═══════════════════════════════════════════════════
        // MÉTODOS CRUD VIRTUALES (pueden ser overrideados)
        // ═══════════════════════════════════════════════════

        /// <summary>
        /// Obtiene un DTO por ID con validaciones
        /// </summary>
        public virtual async Task<TDto> GetByIdAsync(int id)
        {
            // 1️⃣ VALIDAR ENTRADA
            if (id <= 0)
                throw new ArgumentException("ID debe ser mayor a 0");

            // 2️⃣ CONSULTAR
            var item = await _repository.GetByIdAsync(id);

            // 3️⃣ VALIDAR QUE EXISTE
            if (item == null)
                throw new KeyNotFoundException($"Registro con ID {id} no encontrado");

            return item;
        }

        /// <summary>
        /// Obtiene todos los registros
        /// </summary>
        public virtual async Task<List<TDto>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        /// <summary>
        /// Crea un nuevo registro con validaciones
        /// </summary>
        public virtual async Task<TDto> CreateAsync(TDto dto)
        {
            // 1️⃣ VALIDAR DATOS (llama al hook virtual)
            ValidateData(dto);

            // 2️⃣ GUARDAR
            return await _repository.AddAsync(dto);
        }

        /// <summary>
        /// Actualiza un registro con validaciones
        /// </summary>
        public virtual async Task<TDto> UpdateAsync(TDto dto)
        {
            // 1️⃣ VALIDAR QUE EXISTE
            var existing = await _repository.GetByIdAsync(dto.Id);
            if (existing == null)
                throw new KeyNotFoundException("Registro no encontrado");

            // 2️⃣ VALIDAR DATOS (llama al hook virtual)
            ValidateData(dto);

            // 3️⃣ ACTUALIZAR
            return await _repository.UpdateAsync(dto);
        }

        /// <summary>
        /// Elimina un registro
        /// </summary>
        public virtual async Task<bool> DeleteAsync(int id)
        {
            // 1️⃣ VALIDAR QUE EXISTE
            var existing = await _repository.GetByIdAsync(id);
            if (existing == null)
                throw new KeyNotFoundException("Registro no encontrado");

            // 2️⃣ ELIMINAR
            return await _repository.DeleteAsync(id);
        }

        // ═══════════════════════════════════════════════════
        // HOOK VIRTUAL PARA VALIDACIÓN (Subclases pueden override)
        // ═══════════════════════════════════════════════════

        /// <summary>
        /// Hook virtual para que las subclases validen datos específicos
        /// </summary>
        protected virtual void ValidateData(TDto dto)
        {
            // Implementación por defecto vacía
            // Las subclases pueden hacer override para agregar validaciones
        }
    }
}
```

---

## Paso 3: Crear Interfaces de Servicio que Heredan

### Ejemplo: IUserService

**Archivo:** `Bussines/Interfaces/IUserService.cs`

```csharp
using Entity.Dtos;

namespace Bussines.Interfaces
{
    /// <summary>
    /// Interfaz de usuario que hereda de IGenericService<UserDto>
    /// </summary>
    public interface IUserService : IGenericService<UserDto>
    {
        // ✅ HEREDADOS DE IGenericService<UserDto>:
        // - GetByIdAsync(id)
        // - GetAllAsync()
        // - CreateAsync(dto)
        // - UpdateAsync(dto)
        // - DeleteAsync(id)

        // ✅ MÉTODOS ESPECÍFICOS DEL USUARIO
        Task<UserDto> GetByEmailAsync(string email);
        Task<UserDto> GetByUsernameAsync(string username);
        Task<List<UserDto>> GetUsersByRoleAsync(int roleId);
    }
}
```

**¿Qué cambió?**
- Antes: `public interface IUserService` (sin herencia)
- Ahora: `public interface IUserService : IGenericService<UserDto>` (hereda CRUD)
- Resultado: **NO declaramos GetByIdAsync, GetAllAsync, etc. aquí** (están en IGenericService)

### Ejemplo: IRoleService

```csharp
using Entity.Dtos;

namespace Bussines.Interfaces
{
    public interface IRoleService : IGenericService<RoleDto>
    {
        // HEREDADOS: GetByIdAsync, GetAllAsync, CreateAsync, UpdateAsync, DeleteAsync
        
        // ESPECÍFICOS:
        Task<RoleDto> GetByNameAsync(string name);
        Task<List<RoleDto>> GetRolesByUserAsync(int userId);
    }
}
```

---

## Paso 4: Implementar Servicios que Heredan

### Ejemplo: UserService

**Archivo:** `Bussines/Services/UserService.cs`

```csharp
using Entity.Dtos;
using Data.Interfaces;
using Bussines.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace Bussines.Services
{
    /// <summary>
    /// Servicio de usuario que hereda CRUD de GenericService<UserDto>
    /// </summary>
    public class UserService : GenericService<UserDto>, IUserService
    {
        // ✅ Repositorio específico (para métodos especiales)
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository) 
            : base(userRepository)  // ✅ Llama al constructor base
        {
            _userRepository = userRepository;
        }

        // ═══════════════════════════════════════════════════
        // OVERRIDE: CreateAsync con lógica especial
        // ═══════════════════════════════════════════════════

        public override async Task<UserDto> CreateAsync(UserDto dto)
        {
            // 1️⃣ VALIDAR (llama al hook virtual)
            ValidateData(dto);

            // 2️⃣ VALIDAR REGLA: Email única
            var existing = await _userRepository.GetByEmailAsync(dto.Email);
            if (existing != null)
                throw new InvalidOperationException("Este email ya está registrado");

            // 3️⃣ VALIDAR REGLA: Username único
            var existingUsername = await _userRepository.GetByUsernameAsync(dto.Username);
            if (existingUsername != null)
                throw new InvalidOperationException("Este nombre de usuario ya existe");

            // 4️⃣ GUARDAR (llama al repositorio base)
            return await _repository.AddAsync(dto);
        }

        // ═══════════════════════════════════════════════════
        // OVERRIDE: UpdateAsync con lógica especial
        // ═══════════════════════════════════════════════════

        public override async Task<UserDto> UpdateAsync(UserDto dto)
        {
            var existing = await _repository.GetByIdAsync(dto.Id);
            if (existing == null)
                throw new KeyNotFoundException("Usuario no encontrado");

            ValidateData(dto);

            // Validar email único (si cambió)
            if (existing.Email != dto.Email)
            {
                var emailExists = await _userRepository.GetByEmailAsync(dto.Email);
                if (emailExists != null)
                    throw new InvalidOperationException("Este email ya está registrado");
            }

            return await _repository.UpdateAsync(dto);
        }

        // ═══════════════════════════════════════════════════
        // OVERRIDE: ValidateData con reglas específicas
        // ═══════════════════════════════════════════════════

        protected override void ValidateData(UserDto dto)
        {
            // Validar username
            if (string.IsNullOrWhiteSpace(dto.Username))
                throw new ArgumentException("Username es requerido");

            if (dto.Username.Length < 3)
                throw new ArgumentException("Username debe tener mínimo 3 caracteres");

            // Validar email
            if (string.IsNullOrWhiteSpace(dto.Email))
                throw new ArgumentException("Email es requerido");

            if (!IsValidEmail(dto.Email))
                throw new ArgumentException("Email no es válido");
        }

        // ═══════════════════════════════════════════════════
        // MÉTODOS ESPECIALES DEL USUARIO
        // ═══════════════════════════════════════════════════

        public async Task<UserDto> GetByEmailAsync(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email es requerido");

            var user = await _userRepository.GetByEmailAsync(email);
            if (user == null)
                throw new KeyNotFoundException("Usuario no encontrado");

            return user;
        }

        public async Task<UserDto> GetByUsernameAsync(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentException("Username es requerido");

            var user = await _userRepository.GetByUsernameAsync(username);
            if (user == null)
                throw new KeyNotFoundException("Usuario no encontrado");

            return user;
        }

        public async Task<List<UserDto>> GetUsersByRoleAsync(int roleId)
        {
            if (roleId <= 0)
                throw new ArgumentException("Role ID debe ser mayor a 0");

            return await _userRepository.GetUsersByRoleAsync(roleId);
        }

        // ═══════════════════════════════════════════════════
        // MÉTODOS AUXILIARES PRIVADOS
        // ═══════════════════════════════════════════════════

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}
```

**¿Qué cambió en UserService?**

| Antes | Ahora |
|-------|-------|
| `public class UserService : IUserService` | `public class UserService : GenericService<UserDto>, IUserService` |
| Implementaba GetByIdAsync, GetAllAsync, etc. | Hereda GetByIdAsync, GetAllAsync, etc. |
| Código duplicado con otros servicios | Código COMPARTIDO de GenericService |
| `public UserService(IUserRepository repo) { ... }` | `public UserService(IUserRepository repo) : base(repo) { ... }` |
| `private void ValidateData() { ... }` | `protected override void ValidateData(UserDto dto) { ... }` |

---

## Paso 5: RoleService Ejemplo Completo

```csharp
using Entity.Dtos;
using Data.Interfaces;
using Bussines.Interfaces;

namespace Bussines.Services
{
    public class RoleService : GenericService<RoleDto>, IRoleService
    {
        private readonly IRoleRepository _roleRepository;

        public RoleService(IRoleRepository roleRepository) 
            : base(roleRepository)
        {
            _roleRepository = roleRepository;
        }

        public override async Task<RoleDto> CreateAsync(RoleDto dto)
        {
            ValidateData(dto);

            // REGLA: Nombre único
            var existing = await _roleRepository.GetByNameAsync(dto.Name);
            if (existing != null)
                throw new InvalidOperationException($"El rol '{dto.Name}' ya existe");

            return await _repository.AddAsync(dto);
        }

        public override async Task<RoleDto> UpdateAsync(RoleDto dto)
        {
            var existing = await _repository.GetByIdAsync(dto.Id);
            if (existing == null)
                throw new KeyNotFoundException("Rol no encontrado");

            ValidateData(dto);

            if (existing.Name != dto.Name)
            {
                var nameExists = await _roleRepository.GetByNameAsync(dto.Name);
                if (nameExists != null)
                    throw new InvalidOperationException($"El rol '{dto.Name}' ya existe");
            }

            return await _repository.UpdateAsync(dto);
        }

        protected override void ValidateData(RoleDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name))
                throw new ArgumentException("Nombre del rol es requerido");

            if (dto.Name.Length < 3)
                throw new ArgumentException("Nombre debe tener mínimo 3 caracteres");
        }

        public async Task<RoleDto> GetByNameAsync(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Nombre es requerido");

            var role = await _roleRepository.GetByNameAsync(name);
            if (role == null)
                throw new KeyNotFoundException("Rol no encontrado");

            return role;
        }

        public async Task<List<RoleDto>> GetRolesByUserAsync(int userId)
        {
            if (userId <= 0)
                throw new ArgumentException("User ID debe ser mayor a 0");

            return await _roleRepository.GetRolesByUserAsync(userId);
        }
    }
}
```

---

## Paso 6: Estructura Completa de Carpetas

```
Bussines/
├── Interfaces/
│   ├── IGenericService.cs          ◄─── NUEVA: Interfaz base CRUD
│   ├── IUserService.cs              ✅ Ahora hereda de IGenericService
│   ├── IRoleService.cs              ✅ Ahora hereda de IGenericService
│   ├── IPersonaService.cs           ✅ Ahora hereda de IGenericService
│   ├── IPermissionService.cs        ✅ Ahora hereda de IGenericService
│   ├── IFormaService.cs             ✅ Ahora hereda de IGenericService
│   ├── IModuloService.cs            ✅ Ahora hereda de IGenericService
│   ├── IModuleFormService.cs        ✅ Ahora hereda de IGenericService
│   ├── IRoleFormPermissionService.cs ✅ Ahora hereda de IGenericService
│   └── IUserRoleService.cs          ⚠️  SIN HERENCIA (lógica especial)
│
├── Services/
│   ├── GenericService.cs            ◄─── NUEVA: Clase base abstracta
│   ├── UserService.cs               ✅ Ahora hereda de GenericService
│   ├── RoleService.cs               ✅ Ahora hereda de GenericService
│   ├── PersonaService.cs            ✅ Ahora hereda de GenericService
│   ├── PermissionService.cs         ✅ Ahora hereda de GenericService
│   ├── FormaService.cs              ✅ Ahora hereda de GenericService
│   ├── ModuloService.cs             ✅ Ahora hereda de GenericService
│   ├── ModuleFormService.cs         ✅ Ahora hereda de GenericService
│   ├── RoleFormPermissionService.cs ✅ Ahora hereda de GenericService
│   └── UserRoleService.cs           ⚠️  Mantiene lógica propia
│
├── Utils/
└── Mapping/
```

---

## ¿Por qué UserRoleService NO hereda de GenericService?

```csharp
// ❌ UserRoleService tiene 3 repositorios (relación muchos-a-muchos)
public UserRoleService(
    IUserRoleRepository userRoleRepository,    // 1
    IUserRepository userRepository,             // 2
    IRoleRepository roleRepository              // 3
)

// GenericService solo acepta 1 repositorio
public GenericService(IGenericRepository<TDto> repository)

// SOLUCIÓN: UserRoleService implementa IUserRoleService directamente
public class UserRoleService : IUserRoleService
{
    // Código personalizado para muchos-a-muchos
    public async Task<bool> AssignRoleToUserAsync(int userId, int roleId) { ... }
    public async Task<bool> RemoveRoleFromUserAsync(int userId, int roleId) { ... }
    public async Task<List<RoleDto>> GetUserRolesAsync(int userId) { ... }
    public async Task<bool> UserHasRoleAsync(int userId, int roleId) { ... }
}
```

---

## Paso 7: Registrar en Program.cs

```csharp
// SERVICIOS DE BUSINESS
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IPersonaService, PersonaService>();
builder.Services.AddScoped<IPermissionService, PermissionService>();
builder.Services.AddScoped<IFormaService, FormaService>();
builder.Services.AddScoped<IModuloService, ModuloService>();
builder.Services.AddScoped<IModuleFormService, ModuleFormService>();
builder.Services.AddScoped<IRoleFormPermissionService, RoleFormPermissionService>();
builder.Services.AddScoped<IUserRoleService, UserRoleService>();
```

---

## Ventajas de GenericService<T>

| Antes (sin GenericService) | Después (con GenericService) |
|----------------------------|------------------------------|
| ❌ Cada servicio implementa CRUD | ✅ CRUD una sola vez |
| ❌ ~150 líneas por servicio | ✅ ~40 líneas por servicio |
| ❌ Duplicación de código | ✅ DRY (Don't Repeat Yourself) |
| ❌ Difícil mantener cambios | ✅ Cambio en una clase afecta 8 servicios |
| ❌ Inconsistencias en validación | ✅ Validación consistente |
| ❌ Usuarios pueden implementar diferente | ✅ Patrón estándar para todos |

---

## Template: Crear Nuevo Servicio Rápido

```csharp
// 1️⃣ INTERFAZ
public interface I[Entidad]Service : IGenericService<[Entidad]Dto>
{
    Task<[Entidad]Dto> GetBySpecialPropertyAsync(string specialProperty);
}

// 2️⃣ IMPLEMENTACIÓN
public class [Entidad]Service : GenericService<[Entidad]Dto>, I[Entidad]Service
{
    private readonly I[Entidad]Repository _repository;

    public [Entidad]Service(I[Entidad]Repository repository) 
        : base(repository)
    {
        _repository = repository;
    }

    protected override void ValidateData([Entidad]Dto dto)
    {
        // Validaciones específicas
    }

    public async Task<[Entidad]Dto> GetBySpecialPropertyAsync(string specialProperty)
    {
        // Método específico
        return await _repository.GetBySpecialPropertyAsync(specialProperty);
    }
}

// 3️⃣ REGISTRAR EN Program.cs
builder.Services.AddScoped<I[Entidad]Service, [Entidad]Service>();
```

---

## Flujo Completo: De Request a Response

```
1️⃣ CLIENT (Frontend)
   POST /api/users
   { "username": "juan", "email": "juan@test.com" }
   
2️⃣ CONTROLLER
   [HttpPost]
   public async Task<ActionResult> CreateUser([FromBody] UserDto dto)
   {
       var user = await _userService.CreateAsync(dto);
       return Created(...);
   }

3️⃣ SERVICE (UserService : GenericService)
   public override async Task<UserDto> CreateAsync(UserDto dto)
   {
       ValidateData(dto);  // ◄─── Llama ValidateData override
       
       var existing = await _userRepository.GetByEmailAsync(dto.Email);
       if (existing != null)
           throw new InvalidOperationException(...);
       
       return await _repository.AddAsync(dto);  // ◄─── Usa GenericService
   }

4️⃣ REPOSITORY
   public async Task<UserDto> AddAsync(UserDto dto)
   {
       var entity = _mapper.Map<User>(dto);
       await _dbSet.AddAsync(entity);
       await _context.SaveChangesAsync();
       return _mapper.Map<UserDto>(entity);
   }

5️⃣ DATABASE
   INSERT INTO Users ... VALUES (...)

6️⃣ RESPONSE
   201 Created
   {
     "id": 1,
     "username": "juan",
     "email": "juan@test.com"
   }
```

---

## Comparación: Métodos Override vs Hook Virtual

### Override de Método (Reemplazar completamente)

```csharp
// ANTES:
public override async Task<UserDto> CreateAsync(UserDto dto)
{
    ValidateData(dto);
    
    var existing = await _userRepository.GetByEmailAsync(dto.Email);
    if (existing != null)
        throw new InvalidOperationException("Email ya existe");
    
    return await _repository.AddAsync(dto);
}

// GENÉRICO SIN OVERRIDE:
public virtual async Task<RoleDto> CreateAsync(RoleDto dto)
{
    ValidateData(dto);
    return await _repository.AddAsync(dto);
}
```

### Hook Virtual (Extender sin reemplazar)

```csharp
// GENÉRICO (GenericService):
public virtual async Task<TDto> CreateAsync(TDto dto)
{
    ValidateData(dto);  // ◄─── Hook aquí
    return await _repository.AddAsync(dto);
}

protected virtual void ValidateData(TDto dto)
{
    // Vacío por defecto
}

// ESPECÍFICO (UserService):
protected override void ValidateData(UserDto dto)
{
    // Email único
    var existing = await _userRepository.GetByEmailAsync(dto.Email);
    if (existing != null)
        throw new InvalidOperationException("Email ya existe");
}
```

---

## Errores Comunes y Soluciones

### Error 1: "Constructor debe tener base(repository)"
```csharp
// ❌ INCORRECTO
public class UserService : GenericService<UserDto>
{
    public UserService(IUserRepository repo)
    {
        // Falta : base(repo)
    }
}

// ✅ CORRECTO
public class UserService : GenericService<UserDto>
{
    public UserService(IUserRepository repo) : base(repo)
    {
    }
}
```

### Error 2: "ValidateData no puede ser privado"
```csharp
// ❌ INCORRECTO
private void ValidateData(UserDto dto) { }

// ✅ CORRECTO
protected override void ValidateData(UserDto dto) { }
```

### Error 3: "No puede heredar de GenericService<RoleDto>"
```csharp
// Asegúrate que el repositorio hereda de IGenericRepository<T>
// ❌ INCORRECTO
public class RoleRepository : IRoleRepository { }

// ✅ CORRECTO
public class RoleRepository : GenericRepository<RoleDto>, IRoleRepository { }
```

---

## Checklist: Crear un Servicio con GenericService

- [ ] Crear interfaz que hereda de `IGenericService<TDto>`
- [ ] Crear clase que hereda de `GenericService<TDto>`
- [ ] Inyectar repositorio en constructor
- [ ] Llamar `base(repository)` en constructor
- [ ] Override `ValidateData()` con validaciones específicas
- [ ] Override `CreateAsync()` si tiene lógica especial
- [ ] Override `UpdateAsync()` si tiene lógica especial
- [ ] Implementar métodos especiales de la interfaz
- [ ] Registrar en Program.cs con `AddScoped`
- [ ] Probar en Postman

---

## 🎉 Conclusión

**GenericService<T>** es un patrón que:
- ✅ Elimina duplicación de código
- ✅ Hace el código más mantenible
- ✅ Garantiza consistencia
- ✅ Acelera el desarrollo
- ✅ Facilita los cambios futuros

**8 servicios (UserService, RoleService, etc.) ahora comparten el mismo CRUD base. Solo implementan su lógica especial.**
