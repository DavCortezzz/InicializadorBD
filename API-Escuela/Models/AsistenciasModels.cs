using Escuela.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace API_Escuela.Models
{
    public class AsistenciasDto
    {

        [Required(ErrorMessage = "La CURP es un campo obligatorio")]
        [StringLength(18, MinimumLength = 18, ErrorMessage = "La CURP debe tener exactamente 18 caracteres")]
        [RegularExpression(@"^[A-Z]{4}[0-9]{6}[H,M][A-Z]{5}[A-Z0-9]{2}$", ErrorMessage = "El formato de la CURP es inválido")]
        public string idCURP { get; set; }

        [Required(ErrorMessage = "La fecha es requerida")]
        [DataType(DataType.Date)]
        public DateTime fecha { get; set; }

        [Required(ErrorMessage = "Debes especificar el estado.")]
        [RegularExpression("^(Atendio|Falta|Justificado)$",
            ErrorMessage = "El estado debe ser 'Atendio', 'Falta' o 'Justificado'.")]
        public string estado { get; set; }
    }
    public class AsistenciaRequestDto
    {
        [Required(ErrorMessage = "El ID del día es obligatorio")]
        public int fecha { get; set; }

        [Required(ErrorMessage = "La lista de asistencias no puede estar vacía")]
        [MinLength(1, ErrorMessage = "Debes enviar al menos una asistencia")]
        public List<AsistenciasDto> Asistencias { get; set; }
    }
    public class ModificarAsistenciasDto
    {
       [Required(ErrorMessage = "El ID del día es obligatorio")]
       public int asistenciaId { get; set; }

        [Required(ErrorMessage = "Debes especificar el estado de la asistencia.")]
        [RegularExpression("^(Atendio|Falta|Justificado)$",
          ErrorMessage = "El tipo de usuario debe ser 'Direccion' o 'Profesor'.")]
        public string estado { get; set; }
    }
    public class ModificarAsistenciasRequestDto
    {
        [Required(ErrorMessage = "La lista de asistencias no puede estar vacía")]
        [MinLength(1, ErrorMessage = "Debes enviar al menos una asistencia")]
        public List<ModificarAsistenciasDto> Asistencias { get; set; }
    }

}