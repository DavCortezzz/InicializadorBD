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
        public string IdCURP { get; set; }
        public DateTime Fecha { get; set; }
        public string Estado { get; set; }
        public virtual Alumno Alumno { get; set; }

    }

}
