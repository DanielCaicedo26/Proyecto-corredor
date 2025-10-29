using Entity.Entities;
using Microsoft.EntityFrameworkCore;

namespace Entity.DBcontext
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }
    public DbSet<Permission> Permissions { get; set; }
    public DbSet<Persona> Personas { get; set; }
    public DbSet<Forma> Formas { get; set; }
    public DbSet<Modulo> Modulos { get; set; }
    public DbSet<ModuleForm> ModuleForms { get; set; }
    public DbSet<RoleFormPermission> RoleFormPermissions { get; set; }

    public DbSet<Musica> Musicas { get; set; }
    public DbSet<UserMusica> UserMusicas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuración de UserRole (muchos a muchos con clave compuesta)
            modelBuilder.Entity<UserRole>()
                .HasKey(ur => new { ur.UserId, ur.RoleId });

            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.User)
                .WithMany(u => u.UserRoles)
                .HasForeignKey(ur => ur.UserId);

            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.Role)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(ur => ur.RoleId);

            // Configuración de ModuleForm (muchos a muchos)
            modelBuilder.Entity<ModuleForm>()
                .HasOne(mf => mf.Modulo)
                .WithMany(m => m.ModuleForms)
                .HasForeignKey(mf => mf.ModuloId);

            modelBuilder.Entity<ModuleForm>()
                .HasOne(mf => mf.Forma)
                .WithMany(f => f.ModuleForms)
                .HasForeignKey(mf => mf.FormaId);

            // Configuración de RoleFormPermission
            modelBuilder.Entity<RoleFormPermission>()
                .HasOne(rfp => rfp.Role)
                .WithMany()
                .HasForeignKey(rfp => rfp.RoleId);

            modelBuilder.Entity<RoleFormPermission>()
                .HasOne(rfp => rfp.Forma)
                .WithMany()
                .HasForeignKey(rfp => rfp.FormaId);

            modelBuilder.Entity<RoleFormPermission>()
                .HasOne(rfp => rfp.Permission)
                .WithMany()
                .HasForeignKey(rfp => rfp.PermissionId);

            // Configuración de Persona-User
            modelBuilder.Entity<User>()
                .HasOne(u => u.Persona)
                .WithMany(p => p.Users)
                .HasForeignKey(u => u.PersonaId);

            // Configuración de UserMusica (muchos a muchos)
            modelBuilder.Entity<UserMusica>()
                .HasKey(um => new { um.UserId, um.MusicaId });

            modelBuilder.Entity<UserMusica>()
                .HasOne(um => um.User)
                .WithMany(u => u.UserMusicas)
                .HasForeignKey(um => um.UserId);

            modelBuilder.Entity<UserMusica>()
                .HasOne(um => um.Musica)
                .WithMany(m => m.UserMusicas)
                .HasForeignKey(um => um.MusicaId);
        }
            
}
}