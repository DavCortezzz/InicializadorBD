using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace Escuela.Models
{
    public class EscuelaContext : DbContext
    {
        public EscuelaContext() : base("name=EscuelaDB") { }

        public DbSet<Alumno> Alumnos { get; set; }
        public DbSet<Grupo> Grupos { get; set; }
        public DbSet<Profesor> Profesores { get; set; }
        public DbSet<Asignatura> Asignaturas { get; set; }
        public DbSet<Calificacion> Calificaciones { get; set; }
        public DbSet<Asistencia> Asistencias { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Direccion> Direcciones { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Entity<Asignatura>()
                .HasRequired(a => a.Usuario)
                .WithMany()
                .HasForeignKey(a => a.UsuarioId)
                .WillCascadeOnDelete(false);

            base.OnModelCreating(modelBuilder);
        }
    }
}