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


var builder = WebApplication.CreateBuilder(args);

// ✅ Configurar JWT Settings desde appsettings
var jwtSettings = new JwtSettings();
builder.Configuration.GetSection("Jwt").Bind(jwtSettings);
builder.Services.AddSingleton(jwtSettings);

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

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
