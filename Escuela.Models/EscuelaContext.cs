using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace Escuela.Models
{
    public class EscuelaContext : DbContext
    {
        public EscuelaContext() : base("name=EscuelaDB") { }

        public DbSet<Alumno> Alumnos { get; set; }
        public DbSet<Grupo> Grupos { get; set; }
        public DbSet<Asignatura> Asignaturas { get; set; }
        public DbSet<Calificacion> Calificaciones { get; set; }
        public DbSet<Asistencia> Asistencias { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            base.OnModelCreating(modelBuilder);
        }
    }
}