using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Escuela.Models
{
    public class Asistencia
    {
        public int AsistenciaId { get; set; }

        public int AlumnoId { get; set; }
        public DateTime Fecha { get; set; }
        public int Estado { get; set; }
        public virtual Alumno Alumno { get; set; }
    }
}
