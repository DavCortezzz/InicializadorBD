using System.Collections.Generic;

namespace Escuela.Models
{
    public class Asignatura
    {
        public int AsignaturaId { get; set; }

        public string Nombre { get; set; }

        public int UsuarioId { get; set; }
        public virtual Usuario Usuario { get; set; }
        public int GrupoId { get; set; }
        public virtual Grupo Grupo { get; set; }
        public virtual ICollection<Calificacion> Calificaciones { get; set; }
    }
}