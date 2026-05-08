using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Escuela.Models
{
    public class Alumno
    {
        [Required(ErrorMessage = "El nombre es obligatorio")]
        public string Nombre { get; set; }
        
        [Key]
        [Required]
        [Index(IsUnique = true)]
        [StringLength(18)]
        public string idCURP { get; set; }

        public DateTime FechaNacimiento { get; set; }

        public string Tutor { get; set; }

        public string TelefonoTutor { get; set; }

        public int GrupoId { get; set; }
        public virtual Grupo Grupo { get; set; }
    }
}