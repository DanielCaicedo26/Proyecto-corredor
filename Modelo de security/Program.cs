using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Entity.DBcontext;
using Data.Mappings;
using Data.Interfaces;
using Data.Repositories;
using Bussines.Interfaces;
using Bussines.Services;
using Modelo_de_security.Settings;
using Modelo_de_security.Middleware;

var builder = WebApplication.CreateBuilder(args);

// ✅ Configurar JWT Settings desde appsettings
var jwtSettings = new JwtSettings();
builder.Configuration.GetSection("Jwt").Bind(jwtSettings);
builder.Services.AddSingleton<IJwtSettings>(jwtSettings);

// ✅ Configurar Logging
builder.Services.AddLogging(config =>
{
    config.ClearProviders();
    config.AddConsole();
    config.AddDebug();
});

// ✅ Configurar CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

// ⚠️ NOTA: JWT aún no está configurado.
// Para habilitarlo, ejecuta: dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer
// Luego descomenta el código de autenticación JWT en Program.cs

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddOpenApi();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// Configurar AutoMapper
builder.Services.AddAutoMapper(typeof(MappingProfile));




// Registrar repositorios - Inyección de dependencias
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IPersonaRepository, PersonaRepository>();
builder.Services.AddScoped<IPermissionRepository, PermissionRepository>();
builder.Services.AddScoped<IFormaRepository, FormaRepository>();
builder.Services.AddScoped<IModuloRepository, ModuloRepository>();
builder.Services.AddScoped<IModuleFormRepository, ModuleFormRepository>();
builder.Services.AddScoped<IRoleFormPermissionRepository, RoleFormPermissionRepository>();
builder.Services.AddScoped<IUserRoleRepository, UserRoleRepository>();

// Registrar servicios de Business Layer
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IPersonaService, PersonaService>();
builder.Services.AddScoped<IPermissionService, PermissionService>();
builder.Services.AddScoped<IFormaService, FormaService>();
builder.Services.AddScoped<IModuloService, ModuloService>();
builder.Services.AddScoped<IModuleFormService, ModuleFormService>();
builder.Services.AddScoped<IRoleFormPermissionService, RoleFormPermissionService>();
builder.Services.AddScoped<IUserRoleService, UserRoleService>();

// ✅ Registrar AuthService
builder.Services.AddScoped<IAuthService, AuthService>();

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// ✅ Middleware de manejo global de excepciones
app.UseMiddleware<GlobalExceptionHandler>();

app.UseHttpsRedirection();

app.UseCors("AllowFrontend");

app.UseAuthorization();

app.MapControllers();

app.Run();
