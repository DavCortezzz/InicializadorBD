using Escuela.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace API_Escuela.Models
{
    public class RegistroAlumnosDto
    {
        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(80, MinimumLength = 3, ErrorMessage = "El nombre debe tener entre 3 y 80 caracteres")]
        [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s]+$", ErrorMessage = "El nombre solo puede contener letras")]
        public string nombre { get; set; }

        [Required(ErrorMessage = "La CURP es un campo obligatorio")]
        [StringLength(18, MinimumLength = 18, ErrorMessage = "La CURP debe tener exactamente 18 caracteres")]
        [RegularExpression(@"^[A-Z]{4}[0-9]{6}[H,M][A-Z]{5}[A-Z0-9]{2}$", ErrorMessage = "El formato de la CURP es inválido")]
        public string idCURP { get; set; }

        [Required(ErrorMessage = "La fecha de nacimiento es requerida")]
        [DataType(DataType.Date, ErrorMessage = "El formato de fecha no es válido")]
        // Ejemplo de validación para evitar fechas futuras o imposibles
        [Range(typeof(DateTime), "1900-01-01", "2026-12-31", ErrorMessage = "La fecha está fuera del rango permitido")]
        public DateTime fechaNacimiento { get; set; }

        [Required(ErrorMessage = "El nombre del tutor es obligatorio")]
        [StringLength(100, ErrorMessage = "El nombre del tutor no puede exceder los 100 caracteres")]
        public string tutor { get; set; }

        [Required(ErrorMessage = "El teléfono del tutor es necesario")]
        [Phone(ErrorMessage = "El formato de teléfono no es válido")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "El teléfono debe ser a 10 dígitos numéricos")]
        public string telefonoTutor { get; set; }

        [Required(ErrorMessage = "Debe especificar el ID del grupo")]
        [Range(1, int.MaxValue, ErrorMessage = "El ID de grupo debe ser un número positivo")]
        public int grupoId { get; set; }

    }

    public class BuscarAlumnoByIdDto
    {
        [Required(ErrorMessage = "El ID del alumno es necesario")]
        public String idCURP { get; set; }
    }

    public class BuscarAlumnosByGroupIdDto
    {
        [Required(ErrorMessage = "El ID del grupo es necesario")]
        [Range(1, int.MaxValue, ErrorMessage = "El ID del alumno debe ser un número positivo")]
        public int idGrupo { get; set; }
    }

    public class ModificarAlumnosDto
    {
        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(80, MinimumLength = 3, ErrorMessage = "El nombre debe tener entre 3 y 80 caracteres")]
        [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s]+$", ErrorMessage = "El nombre solo puede contener letras")]
        public string nombre { get; set; }

        [Required(ErrorMessage = "La CURP es un campo obligatorio")]
        [StringLength(18, MinimumLength = 18, ErrorMessage = "La CURP debe tener exactamente 18 caracteres")]
        [RegularExpression(@"^[A-Z]{4}[0-9]{6}[H,M][A-Z]{5}[A-Z0-9]{2}$", ErrorMessage = "El formato de la CURP es inválido")]
        public string idCURP { get; set; }
        [Required(ErrorMessage = "La nueva CURP es un campo obligatorio")]
        [StringLength(18, MinimumLength = 18, ErrorMessage = "La CURP debe tener exactamente 18 caracteres")]
        [RegularExpression(@"^[A-Z]{4}[0-9]{6}[H,M][A-Z]{5}[A-Z0-9]{2}$", ErrorMessage = "El formato de la CURP es inválido")]
        public string nuevaIdCURP { get; set; }

        [Required(ErrorMessage = "La fecha de nacimiento es requerida")]
        [DataType(DataType.Date, ErrorMessage = "El formato de fecha no es válido")]
        // Ejemplo de validación para evitar fechas futuras o imposibles
        [Range(typeof(DateTime), "1900-01-01", "2026-12-31", ErrorMessage = "La fecha está fuera del rango permitido")]
        public DateTime fechaNacimiento { get; set; }

        [Required(ErrorMessage = "El nombre del tutor es obligatorio")]
        [StringLength(100, ErrorMessage = "El nombre del tutor no puede exceder los 100 caracteres")]
        public string tutor { get; set; }

        [Required(ErrorMessage = "El teléfono del tutor es necesario")]
        [Phone(ErrorMessage = "El formato de teléfono no es válido")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "El teléfono debe ser a 10 dígitos numéricos")]
        public string telefonoTutor { get; set; }

        [Required(ErrorMessage = "Debe especificar el ID del grupo")]
        [Range(1, int.MaxValue, ErrorMessage = "El ID de grupo debe ser un número positivo")]
        public int grupoId { get; set; }

    }

    public class ObtenerAlumnoPorIdCURP
    {
        [Required(ErrorMessage = "La CURP del alumno es necesaria")]
        [StringLength(18, MinimumLength = 18, ErrorMessage = "La CURP debe tener exactamente 18 caracteres")]
        [RegularExpression(@"^[A-Z]{4}[0-9]{6}[H,M][A-Z]{5}[A-Z0-9]{2}$", ErrorMessage = "El formato de la CURP es inválido")]
        public string idCURP { get; set; }
    }
    
    public class ObtenerAlumnosPorGrupoDto
    {
        [Required(ErrorMessage = "El ID del grupo es necesario")]
        [Range(1, int.MaxValue, ErrorMessage = "El ID del grupo debe ser un número positivo")]
        public int grupoId { get; set; }
    }
}