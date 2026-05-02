using System.Collections.Generic;

namespace Escuela.Models
{
    public class Asignatura
    {
        public int AsignaturaId { get; set; }

        public string Nombre { get; set; }

        public int ProfesorId { get; set; }
        public virtual Profesor Profesor { get; set; }
        public virtual ICollection<Calificacion> Calificaciones { get; set; }
    }
}