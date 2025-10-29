using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
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
            .WithOrigins("http://localhost:3000", "http://localhost:4200", "http://localhost:5173")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
    });
});

// ✅ Configurar JWT Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings.Issuer,
        ValidAudience = jwtSettings.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey)),
        ClockSkew = TimeSpan.Zero
    };
});

// Add services to the container.
builder.Services.AddControllers();

// ✅ Configurar Swagger (Swashbuckle)
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Modelo de Security API",
        Version = "v1",
        Description = "API para gestionar usuarios, roles, permisos, formas y módulos",
        Contact = new Microsoft.OpenApi.Models.OpenApiContact
        {
            Name = "Developer Team",
            Email = "info@ejemplo.com"
        }
    });

    // Agregar documentación XML de comentarios
    var xmlFile = System.IO.Path.Combine(System.AppContext.BaseDirectory, "Modelo de security.xml");
    if (System.IO.File.Exists(xmlFile))
    {
        c.IncludeXmlComments(xmlFile);
    }

    // Configurar Swagger para JWT
    var securityScheme = new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "JWT Authentication",
        Description = "Introduce tu token JWT en el campo siguiente",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    };

    c.AddSecurityDefinition("Bearer", securityScheme);
    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
    });
});

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
    // ✅ Swagger UI
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Modelo de Security API v1");
        c.RoutePrefix = string.Empty; // Para abrir Swagger en la raíz (localhost:5000/)
    });
}

// ✅ Middleware de manejo global de excepciones
app.UseMiddleware<GlobalExceptionHandler>();

app.UseHttpsRedirection();

app.UseCors("AllowFrontend");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
