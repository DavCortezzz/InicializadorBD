using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Escuela.Models
{
    public class Asistencia
    {
        public int AsistenciaId { get; set; }
<<<<<<< Updated upstream

        public int AlumnoId { get; set; }
=======
        public string IdCURP { get; set; }

        [Column(TypeName = "date")]
>>>>>>> Stashed changes
        public DateTime Fecha { get; set; }
        public int Estado { get; set; }
        public virtual Alumno Alumno { get; set; }
    }
}
