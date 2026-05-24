using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace API_Escuela.Models
{
    public class AlumnoAsistenciaGridVM
    {
        [Required(ErrorMessage = "El ID CURP del alumno es obligatorio.")]
        [StringLength(18, MinimumLength = 18, ErrorMessage = "El CURP debe tener exactamente 18 caracteres.")]
        public string idCURP { get; set; }

        public string nombreAlumno { get; set; } 

        [Required(ErrorMessage = "Debes especificar el estado de cada alumno.")]
        [RegularExpression("^(Atendio|Falta|Justificado)$",
            ErrorMessage = "El estado debe ser obligatoriamente 'Atendio', 'Falta' o 'Justificado'.")]
        public string estado { get; set; }
    }

    public class CrearAsistenciasDto
    {
        [Required(ErrorMessage = "El grado del grupo es obligatorio.")]
        public string grupoGrado { get; set; }

        [Required(ErrorMessage = "La fecha de asistencia es requerida.")]
        [DataType(DataType.Date)]
        public DateTime fecha { get; set; }
    }

    public class ModificarAsistenciasDto
    {
        [Required(ErrorMessage = "El grado del grupo es obligatorio para identificar los registros.")]
        public string grupoGrado { get; set; }

        [Required(ErrorMessage = "La fecha es requerida para saber qué día se va a modificar.")]
        [DataType(DataType.Date)]
        public DateTime fecha { get; set; }

        [Required(ErrorMessage = "La lista de asistencias no puede ir vacía.")]
        public List<AlumnoAsistenciaGridVM> AlumnosAsistencia { get; set; }
    }

    public class ConsultarAsistenciaDto
    {
        [Required(ErrorMessage = "El grado del grupo es obligatorio para realizar la búsqueda.")]
        public string grupoGrado { get; set; }

        [Required(ErrorMessage = "La fecha es obligatoria para consultar la lista.")]
        [DataType(DataType.Date)]
        public DateTime fecha { get; set; }
    }
}