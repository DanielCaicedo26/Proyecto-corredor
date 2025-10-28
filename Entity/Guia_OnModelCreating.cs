using Entity.Entities;
using Microsoft.EntityFrameworkCore;

/// <summary>
/// GUÍA COMPLETA: Cómo configurar relaciones en OnModelCreating
/// </summary>
public class ApplicationDbContextExample : DbContext
{
    public ApplicationDbContextExample(DbContextOptions<ApplicationDbContextExample> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // ============================================
        // PATRÓN 1: Relación UNO a MUCHOS (1:M)
        // ============================================
        // Un Persona puede tener muchos Users
        // Pero un User tiene solo UN Persona
        
        modelBuilder.Entity<User>()
            .HasOne(u => u.Persona)              // Un User tiene UN Persona
            .WithMany(p => p.Users)              // Un Persona tiene MUCHOS Users
            .HasForeignKey(u => u.PersonaId);   // La FK es PersonaId
        
        // Resultado en BD:
        // Tabla Users: Id, PersonaId (FK), Username, ...
        // Tabla Personas: Id, Name, ...
        // PersonaId en Users apunta a Id en Personas


        // ============================================
        // PATRÓN 2: Relación MUCHOS a MUCHOS (M:M)
        // ============================================
        // Un User puede tener muchos Roles
        // Un Role puede tener muchos Users
        // Necesita tabla intermedia: UserRole
        
        modelBuilder.Entity<UserRole>()
            .HasKey(ur => new { ur.UserId, ur.RoleId }); // CLAVE PRIMARIA COMPUESTA
        
        modelBuilder.Entity<UserRole>()
            .HasOne(ur => ur.User)
            .WithMany(u => u.UserRoles)
            .HasForeignKey(ur => ur.UserId);
        
        modelBuilder.Entity<UserRole>()
            .HasOne(ur => ur.Role)
            .WithMany(r => r.UserRoles)
            .HasForeignKey(ur => ur.RoleId);
        
        // Resultado en BD:
        // Tabla UserRoles: UserId (FK), RoleId (FK), Id
        // Clave primaria: UserId + RoleId (no se puede repetir)


        // ============================================
        // PATRÓN 3: Relación UNO a UNO (1:1)
        // ============================================
        // Un User tiene UN perfil, un Perfil pertenece a UN User
        // (Ejemplo si tuvieras una entidad Profile)
        
        // modelBuilder.Entity<User>()
        //     .HasOne(u => u.Profile)
        //     .WithOne(p => p.User)
        //     .HasForeignKey<Profile>(p => p.UserId);


        // ============================================
        // PATRÓN 4: Comportamiento al eliminar
        // ============================================
        // Qué pasa cuando eliminas un Persona
        
        modelBuilder.Entity<User>()
            .HasOne(u => u.Persona)
            .WithMany(p => p.Users)
            .HasForeignKey(u => u.PersonaId)
            .OnDelete(DeleteBehavior.Cascade); // Si elimino Persona, elimino sus Users
        
        // DeleteBehavior opciones:
        // - Cascade: Elimina registros relacionados
        // - Restrict: No permite eliminar si hay relacionados
        // - SetNull: Pone NULL en la FK
        // - ClientSetNull: Igual que SetNull pero en la aplicación


        // ============================================
        // PATRÓN 5: Propiedades adicionales
        // ============================================
        
        modelBuilder.Entity<User>()
            .Property(u => u.Username)
            .IsRequired()                    // No permite NULL
            .HasMaxLength(100);             // Máximo 100 caracteres
        
        modelBuilder.Entity<User>()
            .Property(u => u.Email)
            .HasMaxLength(256);
        
        modelBuilder.Entity<Role>()
            .Property(r => r.Name)
            .IsRequired()
            .HasMaxLength(50);


        // ============================================
        // PATRÓN 6: Índices (búsquedas más rápidas)
        // ============================================
        
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique(); // Email debe ser único
        
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Username);


        // ============================================
        // PATRÓN 7: Valores por defecto
        // ============================================
        
        // Ejemplo: Configurar propiedades de auditoría
        // Los campos Created y Updated se heredan de BaseEntity
        // y se pueden configurar aquí si es necesario
    }
}

// ============================================
// RESUMEN VISUAL DE RELACIONES
// ============================================

/*
UNO A MUCHOS (1:M):
┌─────────────┐        ┌──────────────┐
│  Personas   │◄───────│    Users     │
│  Id (PK)    │   1:M  │ PersonaId(FK)│
└─────────────┘        └──────────────┘

MUCHOS A MUCHOS (M:M):
┌─────────────┐        ┌──────────────┐        ┌────────────┐
│    Users    │────────│  UserRoles   │────────│   Roles    │
│ Id (PK)     │   1:M  │(UserId, RoleId)│  M:1 │ Id (PK)    │
└─────────────┘        └──────────────┘        └────────────┘
     (tabla intermedia)

UNO A UNO (1:1):
┌─────────────┐        ┌──────────────┐
│    Users    │◄───────│   Profiles   │
│ Id (PK)     │   1:1  │ UserId (FK)  │
└─────────────┘        └──────────────┘
*/

// ============================================
// PRÁCTICA: Cómo leer configuraciones
// ============================================

/*
Pregunta: ¿Qué significa esto?
    modelBuilder.Entity<UserRole>()
        .HasKey(ur => new { ur.UserId, ur.RoleId });

Respuesta:
✓ Trabaja con la tabla UserRole
✓ Define clave primaria compuesta
✓ Clave = UserId + RoleId (dos campos juntos)
✓ No se puede repetir la combinación (1,1), (1,2) pero no (1,1) dos veces

---

Pregunta: ¿Qué significa esto?
    modelBuilder.Entity<User>()
        .HasOne(u => u.Persona)
        .WithMany(p => p.Users)
        .HasForeignKey(u => u.PersonaId);

Respuesta:
✓ Trabaja con la tabla User
✓ Un User tiene UN Persona
✓ Un Persona tiene MUCHOS Users
✓ La columna PersonaId en Users apunta a Id en Personas
*/
