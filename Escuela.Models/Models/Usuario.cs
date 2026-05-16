using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Escuela.Models
{
    public class Usuario
    {
        public int UsuarioId { get; set; }

        public string Nombre { get; set; }
        public string Correo { get; set; }
        public string Contrasena { get; set; }
        public string TipoUsuario { get; set; }

        public string CodigoVerificacion { get; set; }

        public DateTime FechaExpiracionCodigo { get; set; }
    }
}
