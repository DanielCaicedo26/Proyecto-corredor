# ✅ Checklist de Autenticación

## Estado Actual del Proyecto

### ✅ Completado

- [x] **DTOs de Autenticación**
  - [x] LoginRequest.cs
  - [x] RegisterRequest.cs
  - [x] AuthResponse.cs

- [x] **Servicio de Autenticación**
  - [x] IAuthService.cs
  - [x] AuthService.cs
  - [x] Login con username + password
  - [x] Registro con validaciones
  - [x] Hasheo de contraseñas (SHA256)
  - [x] Generación de JWT token

- [x] **Controladores**
  - [x] AuthController.cs (login/register)
  - [x] UsersController.cs (CRUD usuarios)

- [x] **Configuración**
  - [x] CORS habilitado
  - [x] AuthService registrado en DI
  - [x] appsettings.json con JwtSettings
  - [x] Program.cs actualizado

- [x] **Documentación**
  - [x] AUTENTICACION_RESUMEN.md
  - [x] Program-JWT-SETUP.md
  - [x] IMPLEMENTACION_LISTA.md
  - [x] COMO_USAR_AUTHORIZE.md
  - [x] Postman-API.json

- [x] **Seguridad**
  - [x] Contraseñas hasheadas
  - [x] Validaciones de entrada
  - [x] Logging implementado
  - [x] CORS configurado

---

### ⏳ Pendiente: Instalar JWT

```powershell
cd "c:\Users\User\source\repos\Proyecto corredor\Modelo de security"
dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer
```

**Después de instalar:**
- [ ] Uncomment JWT config en Program.cs
- [ ] Agregar `using Microsoft.AspNetCore.Authentication.JwtBearer;`
- [ ] Probar endpoints

---

### 📋 Verificación Antes de Usar

#### 1. Verificar que AuthService esté en Program.cs
```csharp
builder.Services.AddScoped<IAuthService, AuthService>();
```
✅ **Estado:** Completado

#### 2. Verificar CORS en Program.cs
```csharp
app.UseCors("AllowFrontend");
```
✅ **Estado:** Completado

#### 3. Verificar appsettings.json
```json
"Jwt": {
    "SecretKey": "...",
    "Issuer": "...",
    "Audience": "...",
    "ExpirationMinutes": 60
}
```
✅ **Estado:** Completado

#### 4. Verificar UserDto tiene Password
```csharp
public string? Password { get; set; }
```
✅ **Estado:** Completado

---

## 🚀 Pasos para Activar JWT

### Paso 1: Instalar Paquete
```bash
dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer
```

### Paso 2: En Program.cs, descomentar JWT
Reemplazar esta línea:
```csharp
// ⚠️ NOTA: JWT aún no está configurado.
```

Con:
```csharp
// ✅ Configurar Autenticación JWT
builder.Services
    .AddAuthentication("Bearer")
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey)),
            ValidateIssuer = true,
            ValidIssuer = jwtSettings.Issuer,
            ValidateAudience = true,
            ValidAudience = jwtSettings.Audience,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };
    });

// ✅ Configurar Autorización
builder.Services.AddAuthorization();
```

### Paso 3: Agregar using en Program.cs
```csharp
using Microsoft.AspNetCore.Authentication.JwtBearer;
```

### Paso 4: Uncomment el middleware
Reemplazar:
```csharp
app.UseAuthorization();
```

Con:
```csharp
app.UseAuthentication();
app.UseAuthorization();
```

---

## 🧪 Pruebas a Realizar (Una Vez Instalado JWT)

### Test 1: Registro
```bash
curl -X POST http://localhost:5000/api/auth/register \
  -H "Content-Type: application/json" \
  -d '{"username":"test","email":"test@test.com","password":"pass123","confirmPassword":"pass123"}'
```
**Resultado esperado:** 201 Created con usuario

### Test 2: Login
```bash
curl -X POST http://localhost:5000/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{"username":"test","password":"pass123"}'
```
**Resultado esperado:** 200 OK con token JWT

### Test 3: Usar Token
```bash
curl -X GET http://localhost:5000/api/users \
  -H "Authorization: Bearer {TOKEN_AQUI}"
```
**Resultado esperado:** 200 OK con lista de usuarios

### Test 4: Sin Token
```bash
curl -X GET http://localhost:5000/api/users
```
**Resultado esperado:** 401 Unauthorized

---

## 📊 Arquitectura de Autenticación

```
┌─────────────────────────────────────────────┐
│         Cliente (Angular/React/etc)         │
└────────────────┬────────────────────────────┘
                 │
        POST /api/auth/login
        (username, password)
                 │
                 ▼
        ┌────────────────────────────┐
        │   AuthController          │
        │   - ValidaCredenciales    │
        └────────────┬───────────────┘
                     │
                     ▼
        ┌────────────────────────────┐
        │   AuthService             │
        │   - VerifyPassword()       │
        │   - GenerateJwtToken()    │
        └────────────┬───────────────┘
                     │
                     ▼
        ┌────────────────────────────┐
        │   Respuesta                │
        │   {                        │
        │     token: "eyJ...",       │
        │     user: {...}            │
        │   }                        │
        └────────────┬───────────────┘
                     │
                     ▼
        ┌────────────────────────────┐
        │   Cliente Almacena Token   │
        │   (LocalStorage/SessionS.)│
        └────────────────────────────┘
                     │
        GET /api/users
        Header: Authorization: Bearer {token}
                     │
                     ▼
        ┌────────────────────────────┐
        │   Middleware JWT           │
        │   - Valida Token           │
        │   - Extrae Claims          │
        └────────────┬───────────────┘
                     │
                     ▼
        ┌────────────────────────────┐
        │   UsersController          │
        │   [Authorize]              │
        │   GetAll()                 │
        └────────────┬───────────────┘
                     │
                     ▼
        ┌────────────────────────────┐
        │   Respuesta (200 OK)       │
        │   [users...]               │
        └────────────────────────────┘
```

---

## 📚 Archivos Importantes

| Archivo | Propósito | Estado |
|---|---|---|
| AuthService.cs | Lógica de auth | ✅ |
| AuthController.cs | Endpoints login/register | ✅ |
| UsersController.cs | CRUD usuarios | ✅ |
| Program.cs | Configuración DI + CORS | ✅ |
| appsettings.json | JWT config | ✅ |
| Program-JWT-SETUP.md | Instrucciones JWT | ✅ |

---

## 🔐 Configuración de Seguridad

| Aspecto | Implementado | Nota |
|---|---|---|
| Hasheo de contraseñas | ✅ | SHA256 |
| JWT token | ✅ | 60 min expiración |
| CORS | ✅ | Permite todos los orígenes |
| Validaciones | ✅ | Email, username, password |
| Logging | ✅ | Intentos de login |
| HTTPS | ⏳ | Configurar en producción |

---

## ⚠️ Notas Importantes

1. **SecretKey en appsettings.json:**
   - Actual: `"your-secret-key-change-this-in-production"`
   - ⚠️ **CAMBIAR EN PRODUCCIÓN**

2. **CORS actualmente abierto:**
   - Permite solicitudes desde cualquier origen
   - ⚠️ **Restringir en producción**

3. **Tokens expiran cada 60 minutos:**
   - Configurable en `appsettings.json`
   - El usuario debe hacer login de nuevo

4. **Las contraseñas NO se envían en respuestas:**
   - Solo el hash se guarda en BD
   - Se devuelve DTO sin password

---

## ✨ Próximas Mejoras (Opcional)

- [ ] Refresh tokens
- [ ] Roles y permisos en JWT
- [ ] Rate limiting en login
- [ ] 2FA (Two-Factor Authentication)
- [ ] OAuth2 / OpenID Connect
- [ ] Logout (blacklist de tokens)
- [ ] Remember me

---

## 📞 Ayuda

Si algo no funciona, revisa:
1. ¿Instalaste JWT? → `dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer`
2. ¿Uncommented JWT en Program.cs?
3. ¿El usuario está registrado?
4. ¿El token es válido? → Verifica expiración
5. ¿El header Authorization es correcto? → `Bearer {token}`

---

## 🎉 ¡Listo!

Tu sistema de autenticación está completo. Solo falta:

1. Instalar JWT
2. Descomentar JWT en Program.cs
3. ¡Probar! 🚀

¿Preguntas? Revisa los archivos `.md` incluidos.
