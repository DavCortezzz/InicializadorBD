using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Escuela.Models
{
    public class Calificacion
    {
        public int CalificacionId { get; set; }

        public int AlumnoId { get; set; }
        public int AsignaturaId { get; set; }

        [Range(0, 10)]
        public int PrimerTrimestre { get; set; }

        [Range(0, 10)]
        public int SegundoTrimestre { get; set; }

        [Range(0, 10)]
        public int TercerTrimestre { get; set; }

        [Range(0, 10)]
        public int CalificacionFinal { get; set; }

        public virtual Alumno Alumno { get; set; }
        public virtual Asignatura Asignatura { get; set; }
    }
}
