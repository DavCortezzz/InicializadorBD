using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace API_Escuela.Models
{
    public class UsuarioDto
    {
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(100, ErrorMessage = "El nombre no puede exceder los 100 caracteres.")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El correo electrónico es indispensable.")]
        [EmailAddress(ErrorMessage = "El formato del correo no es válido.")]
        public string Correo { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        [MinLength(6, ErrorMessage = "La contraseña debe tener al menos 6 caracteres.")]
        public string Contrasena { get; set; }

        [Required(ErrorMessage = "Debes especificar el tipo de usuario.")]
        [RegularExpression("^(Direccion|Profesor)$",
            ErrorMessage = "El tipo de usuario debe ser 'Direccion' o 'Profesor'.")]
        public string tipoUsuario { get; set; }
    }

    public class LoginDto
    {
        [Required(ErrorMessage = "El correo es necesario para iniciar sesión.")]
        [EmailAddress(ErrorMessage = "Formato de correo inválido.")]
        public string Correo { get; set; }

        [Required(ErrorMessage = "La contraseña es necesaria.")]
        public string Contrasena { get; set; }
    }
}
