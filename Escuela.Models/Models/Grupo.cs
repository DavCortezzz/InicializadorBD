using System.Collections.Generic;

namespace Escuela.Models
{
    public class Grupo
    {
        public int GrupoId { get; set; }
        public string Grado { get; set; }
        public string Turno { get; set; }
        public string CicloEscolar { get; set; }

        public virtual ICollection<Alumno> Alumnos { get; set; }
    }
}