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
        public string nombre { get; set; }

        [Required(ErrorMessage = "El correo electrónico es indispensable.")]
        [EmailAddress(ErrorMessage = "El formato del correo no es válido.")]
        public string correo { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        [MinLength(6, ErrorMessage = "La contraseña debe tener al menos 6 caracteres.")]
        public string contrasena { get; set; }

        [Required(ErrorMessage = "Debes especificar el tipo de usuario.")]
        [RegularExpression("^(Direccion|Profesor)$",
            ErrorMessage = "El tipo de usuario debe ser 'Direccion' o 'Profesor'.")]
        public string tipoUsuario { get; set; }
    }
    public class ModificarUsuarioDto
    {
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(100, ErrorMessage = "El nombre no puede exceder los 100 caracteres.")]
        public string nombre { get; set; }

        [Required(ErrorMessage = "El correo electrónico es indispensable.")]
        [EmailAddress(ErrorMessage = "El formato del correo no es válido.")]
        public string correo { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        [MinLength(6, ErrorMessage = "La contraseña debe tener al menos 6 caracteres.")]
        public string contrasena { get; set; }

        [Required(ErrorMessage = "Debes especificar el tipo de usuario.")]
        [RegularExpression("^(Direccion|Profesor)$",
            ErrorMessage = "El tipo de usuario debe ser 'Direccion' o 'Profesor'.")]
        public string tipoUsuario { get; set; }
    }
    public class LoginDto
    {
        [Required(ErrorMessage = "El correo es necesario para iniciar sesión.")]
        [EmailAddress(ErrorMessage = "Formato de correo inválido.")]
        public string correo { get; set; }

        [Required(ErrorMessage = "La contraseña es necesaria.")]
        public string contrasena { get; set; }
    }
    public class deleteUserDto
    {
        [Required(ErrorMessage = "El id del usuario a eliminar es necesario para hacer la accion")]
        public int id { get; set; }
    }

    public class getUsesrsNombreDto {
     public string nombre { get; set; }

        public string tipoUsuario { get; set; }
    }
    public class UsersGridDto {
        public string nombre { get; set; }

        public string correo { get; set; }
        public string tipoUsuario { get; set; }
        public int usuarioId { get; set; }
    }

   public class UsuarioCambioContrasenaDto
         {


        public string correo { get; set; }
        public string contrasena { get; set; }
        public string codigoVerificacion { get; set; }
    };
}
