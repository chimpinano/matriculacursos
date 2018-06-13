using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using prSistemaAC.Models;

namespace prSistemaAC.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }

        public DbSet<prSistemaAC.Models.ApplicationUser> ApplicationUser { get; set; }

        public DbSet<prSistemaAC.Models.Categoria> Categoria { get; set; }

        public DbSet<prSistemaAC.Models.Curso> Curso { get; set; }

        public DbSet<prSistemaAC.Models.Instructor> Instructor { get; set; }

        public DbSet<prSistemaAC.Models.Estudiante> Estudiante { get; set; }

        public DbSet<prSistemaAC.Models.Persona> Persona { get; set; }

        public DbSet<prSistemaAC.Models.Asignacion> Asignacion { get; set; }

        public DbSet<prSistemaAC.Models.Inscripcion> Inscripcion { get; set; }
    }
}
