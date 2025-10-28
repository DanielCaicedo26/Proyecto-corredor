# 🔐 Configuración JWT - Instrucciones de Instalación

## Estado Actual ⚠️
El proyecto tiene **autenticación basic** configurada, pero **JWT no está activado** porque falta instalar el paquete de NuGet.

## ¿Qué hemos hecho?

✅ **Creado:**
- `AuthService.cs` - Servicio de autenticación con login/register
- `AuthController.cs` - Controlador API para login/register
- DTOs de autenticación: `LoginRequest`, `RegisterRequest`, `AuthResponse`
- `Program.cs` actualizado con CORS

⏳ **Pendiente:**
- Instalar paquete JWT
- Habilitar autenticación JWT

---

## Paso 1: Instalar Paquete JWT

Abre PowerShell en el directorio raíz del proyecto y ejecuta:

```powershell
cd "c:\Users\User\source\repos\Proyecto corredor\Modelo de security"
dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer
```

---

## Paso 2: Actualizar Program.cs

Una vez instalado el paquete, reemplaza esta sección en `Program.cs`:

```csharp
// ⚠️ NOTA: JWT aún no está configurado.
// Para habilitarlo, ejecuta: dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer
// Luego descomenta el código de autenticación JWT en Program.cs
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

Y reemplaza:

```csharp
app.UseAuthorization();
```

Con:

```csharp
app.UseAuthentication();
app.UseAuthorization();
```

---

## Paso 3: Agregar usando en Program.cs

Agrega al inicio de `Program.cs`:

```csharp
using Microsoft.AspNetCore.Authentication.JwtBearer;
```

---

## Prueba la API

### 1. Registrar Usuario

**POST** `/api/auth/register`

```json
{
    "username": "user123",
    "email": "user@example.com",
    "password": "password123",
    "confirmPassword": "password123"
}
```

Respuesta:
```json
{
    "success": true,
    "message": "Registro exitoso",
    "user": {
        "id": 1,
        "username": "user123",
        "email": "user@example.com"
    }
}
```

### 2. Login

**POST** `/api/auth/login`

```json
{
    "username": "user123",
    "password": "password123"
}
```

Respuesta:
```json
{
    "success": true,
    "message": "Login exitoso",
    "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
    "user": {
        "id": 1,
        "username": "user123",
        "email": "user@example.com"
    }
}
```

### 3. Usar Token

En las peticiones autorizadas, agrega el header:

```
Authorization: Bearer <token>
```

---

## Cambios Realizados

### 📁 Archivos Creados:
- `Entity/Dtos/Auth/LoginRequest.cs`
- `Entity/Dtos/Auth/RegisterRequest.cs`
- `Entity/Dtos/Auth/AuthResponse.cs`
- `Bussines/Interfaces/IAuthService.cs`
- `Bussines/Services/AuthService.cs`
- `Modelo de security/Controllers/AuthController.cs`

### 📝 Archivos Modificados:
- `Modelo de security/Program.cs` - Agregado CORS y preparado para JWT

### ✅ Ya Configurado:
- `appsettings.json` - Tiene JwtSettings (SecretKey, Issuer, Audience)
- `Modelo de security/Settings/JwtSettings.cs` - Configuración de JWT

---

## ⚠️ Importante

- **Cambiar SecretKey en producción**: El `appsettings.json` tiene una clave de demo
- **Usar HTTPS siempre**: Nunca envíes tokens por HTTP
- **Las contraseñas se hashean** con SHA256 antes de guardarlas

---

## Siguientes Pasos

1. ✅ Instalar JWT
2. ✅ Probar login/register en Postman
3. ⏳ Crear más controladores (Users, Roles, etc.)
4. ⏳ Agregar autorización [Authorize] en endpoints protegidos
5. ⏳ Implementar refresh tokens
