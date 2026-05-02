using System.Collections.Generic;

namespace Escuela.Models
{
    public class Profesor
    {
        public int ProfesorId { get; set; }

        public string Nombre { get; set; }

        public virtual ICollection<Asignatura> Asignaturas { get; set; }
        public int UsuarioId { get; set; }
        public virtual Usuario Usuario { get; set; }
    }
}