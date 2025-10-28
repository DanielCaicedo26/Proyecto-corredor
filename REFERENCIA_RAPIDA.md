# 🚀 Referencia Rápida - Autenticación JWT

## 1️⃣ Instalación (Una Sola Vez)

```powershell
cd "c:\Users\User\source\repos\Proyecto corredor\Modelo de security"
dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer
```

---

## 2️⃣ Endpoints

### 📝 Registrarse (SIN TOKEN)
```http
POST /api/auth/register
Content-Type: application/json

{
  "username": "user123",
  "email": "user@example.com",
  "password": "password123",
  "confirmPassword": "password123"
}
```

### 🔓 Login (SIN TOKEN)
```http
POST /api/auth/login
Content-Type: application/json

{
  "username": "user123",
  "password": "password123"
}
```

**Respuesta:** Obtienes el JWT token

### 👥 Usar Token (CON TOKEN)
```http
GET /api/users
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

---

## 3️⃣ Flujo Simple

```
1. Registrar    POST /api/auth/register
2. Login        POST /api/auth/login       → Obtienes TOKEN
3. Usar TOKEN   GET /api/users             + Header: Authorization: Bearer TOKEN
```

---

## 4️⃣ Códigos de Respuesta

| Código | Significa |
|---|---|
| 200 | ✅ OK |
| 201 | ✅ Creado |
| 400 | ❌ Datos inválidos |
| 401 | ❌ Sin autenticación |
| 500 | ❌ Error servidor |

---

## 5️⃣ Estructura Token JWT

```
eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiI1IiwiYXV0aCI6InVzZXIxMjMiLCJpYXQiOjE2NjY2Njg1NDB9.xyz...

Partes:
1. eyJhb... = Header (algoritmo)
2. eyJz... = Payload (datos)
3. xyz... = Signature (firma)
```

---

## 6️⃣ Validación de Respuestas

### Login Exitoso ✅
```json
{
  "success": true,
  "message": "Login exitoso",
  "token": "eyJhbGci...",
  "user": {
    "id": 1,
    "username": "user123",
    "email": "user@example.com"
  }
}
```

### Error de Autenticación ❌
```json
{
  "success": false,
  "message": "Usuario o contraseña incorrectos"
}
```

### Token Inválido ❌
```
401 Unauthorized
```

---

## 7️⃣ En Postman

1. **Importar:** `Postman-API.json`
2. **Login:** Ejecutar POST /auth/login
3. **Copiar token** de la respuesta
4. **Editar variable** `{{token}}`
5. **Usar en otros endpoints**

---

## 8️⃣ Contraseña Segura

✅ Mínimo 6 caracteres
❌ Menos de 6 caracteres
✅ Se hashea con SHA256
✅ Nunca se devuelve en respuestas

---

## 9️⃣ Expiración del Token

- **Duración:** 60 minutos
- **Cambiar en:** `appsettings.json` → `Jwt.ExpirationMinutes`
- **Si expira:** Login de nuevo

---

## 🔟 Proteger Endpoint

### Sin Protección
```csharp
[HttpGet]
public async Task<ActionResult<List<UserDto>>> GetAll() { }
```

### Con Protección
```csharp
[Authorize]
[HttpGet]
public async Task<ActionResult<List<UserDto>>> GetAll() { }
```

---

## Comando Rápido (curl)

### Registrar
```bash
curl -X POST http://localhost:5000/api/auth/register \
  -H "Content-Type: application/json" \
  -d '{"username":"test","email":"test@test.com","password":"pass123","confirmPassword":"pass123"}'
```

### Login
```bash
curl -X POST http://localhost:5000/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{"username":"test","password":"pass123"}'
```

### Usar Token
```bash
curl -X GET http://localhost:5000/api/users \
  -H "Authorization: Bearer TOKEN_AQUI"
```

---

## Estado Actual

✅ Sistema listo
⏳ Falta instalar JWT
⏳ Falta descomentar config JWT en Program.cs

---

## ¿Qué Falta?

1. `dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer`
2. Uncomment JWT en `Program.cs`
3. Restart API
4. ¡Probar!

---

## 📚 Más Información

- `IMPLEMENTACION_LISTA.md` - Completo
- `Program-JWT-SETUP.md` - Paso a paso JWT
- `CHECKLIST_AUTENTICACION.md` - Verificación
- `COMO_USAR_AUTHORIZE.md` - Proteger endpoints
