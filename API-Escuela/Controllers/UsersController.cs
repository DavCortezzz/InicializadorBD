using API_Escuela.Controllers;
using API_Escuela.Models;
using Escuela.Models;
using System;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Web.Http;
using System.Web.Services.Description;

namespace Escuela.Api.Controllers
{
    public class UsersController : BaseController
    {
        private EscuelaContext db = new EscuelaContext();

        // POST: api/register
        [HttpPost]
        [Route("api/register")]
        public IHttpActionResult Register(UsuarioDto model)
        {
            
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                Usuario usuario  = ValidarAccesoDireccion();
                if (usuario.TipoUsuario != "Direccion")return BadRequest(ModelState);

                if (db.Usuarios.Any(u => u.Correo.ToLower() == model.correo.ToLower()))
                    return Content(System.Net.HttpStatusCode.Conflict, new { message = "El correo electrónico ya está registrado." });

                var user = new Usuario
                {
                    Nombre = model.nombre,
                    Correo = model.correo.ToLower(),
                    Contrasena = EncriptarSHA256(model.contrasena),
                    TipoUsuario = model.tipoUsuario,
                    FechaExpiracionCodigo = DateTime.Now
                };

                db.Usuarios.Add(user);
                db.SaveChanges();

                return Ok(new { message = "Registro exitoso" });
            }
            catch (DbUpdateException)
            {
                return InternalServerError(new Exception("Error al guardar en la base de datos. Verifique los datos."));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return InternalServerError(new Exception("Ocurrió un error inesperado durante el registro. Intentelo de nuevo "));
            }
        }

        // POST: api/update
        [HttpPost]
        [Route("api/update")]
        public IHttpActionResult Update(ModificarUsuarioDto model)
        {

            if (model == null || !ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                Usuario user = ValidarAccesoDireccion();
                if (user == null || user.TipoUsuario != "Direccion")
                    return Content(System.Net.HttpStatusCode.Unauthorized, new { message = "No tiene permisos." });

                if (!string.IsNullOrWhiteSpace(model.nombre)) user.Nombre = model.nombre;
                if (model.contrasena != null) user.Contrasena = EncriptarSHA256(model.contrasena);
                if (!string.IsNullOrWhiteSpace(model.correo)) user.Correo = model.correo;
                if (!string.IsNullOrWhiteSpace(model.tipoUsuario)) user.TipoUsuario = model.tipoUsuario;

                db.SaveChanges();

                return Ok(new { message = "Usuario Propio actualizado correctamente." });
            }
            catch (Exception ex)
            {
                var errorReal = ex.InnerException?.InnerException?.Message ?? ex.InnerException?.Message ?? ex.Message;
                Console.WriteLine(errorReal);
                return InternalServerError(new Exception("Error al guardar: intentelo de nuevo " ));
            }
        }

        // POST: api/getCode
        [HttpPost]
        [Route("api/getCode")]
        public IHttpActionResult SolicitarCodigo(string correo)
        {
            var usuario = db.Usuarios.FirstOrDefault(u => u.Correo == correo);
            if (usuario == null) return NotFound();

            string codigo = new Random().Next(100000, 999999).ToString();

            usuario.CodigoVerificacion = codigo;
            usuario.FechaExpiracionCodigo = DateTime.Now.AddMinutes(15);
            db.SaveChanges();

            EnviarCorreoCodigo(usuario.Correo, codigo);

            return Ok(new { message = "Código enviado al correo." });
        }

        // POST: api/verifyCode
        [HttpPost]
        [Route("api/verifyCode")]
        public IHttpActionResult VerificarCodigo(string correo, string codigo)
        {
            var usuario = db.Usuarios.FirstOrDefault(u => u.Correo == correo);

            if (usuario == null) return NotFound();

            if (usuario.CodigoVerificacion != codigo)
            {
                return BadRequest("El código de verificación es incorrecto.");
            }

            if (usuario.FechaExpiracionCodigo < DateTime.Now)
            {
                return BadRequest("El código ha expirado. Solicita uno nuevo.");
            }
            return Ok(new { message = "Código verificado con éxito. Puede proceder a cambiar la contraseña." });
        }
        // POST: api/resetPassword
        [HttpPost]
        [Route("api/resetPassword")]
        public IHttpActionResult RestablecerContrasena(UsuarioCambioContrasenaDto model)
        {
            var usuario = db.Usuarios.FirstOrDefault(u => u.Correo == model.correo);

            if (usuario == null) return NotFound();

            if (usuario.CodigoVerificacion != model.codigoVerificacion || usuario.FechaExpiracionCodigo < DateTime.Now)
            {
                return BadRequest("Operación no válida o el código ha expirado.");
            }

            usuario.Contrasena = EncriptarSHA256(model.contrasena) ; 
            usuario.CodigoVerificacion = null;
            usuario.FechaExpiracionCodigo = DateTime.Now;

            db.SaveChanges();

            return Ok(new { message = "Contraseña actualizada correctamente." });
        }



        // POST: api/login
        [HttpPost]
        [Route("api/login")]
        public IHttpActionResult Login(LoginDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                string hash = EncriptarSHA256(model.contrasena);
                var user = db.Usuarios.FirstOrDefault(u => u.Correo == model.correo && u.Contrasena == hash);

                if (user == null)
                {
                    var response = Request.CreateResponse(System.Net.HttpStatusCode.Unauthorized, new { message = "Credenciales no Validas" });
                    return ResponseMessage(response);
                }

                var token = JwtService.CreateAccessToken(user.UsuarioId);

                return Ok(new
                {
                    id = user.UsuarioId,
                    tipoUsuario = user.TipoUsuario,
                    token = token
                });
            }
            catch (Exception)
            {
                return InternalServerError(new Exception("Error al procesar el inicio de sesión."));
            }
        }

        // Post: api/users
        [HttpPost]
        [Route("api/users")]
        public IHttpActionResult GetUsers(getUsesrsNombreDto model)
        {
            try
            {
                Usuario user = ValidarAccesoDireccion();
                if (user.TipoUsuario != "Direccion")
                    return Content(System.Net.HttpStatusCode.Unauthorized, new { message = "No tiene permisos para esta acción." });

                var query = db.Usuarios.AsQueryable();
                if (!string.IsNullOrEmpty(model.tipoUsuario))
                {
                    query = query.Where(a => a.TipoUsuario == model.tipoUsuario);
                }
                if (!string.IsNullOrEmpty(model.nombre))
                {
                    string busqueda = model.nombre.ToLower().Trim();
                    query = query.Where(a =>
                        a.Nombre.ToLower().Contains(busqueda));
                }
                

                var resultado = query
                    .OrderBy(a => a.Nombre)
                    .Select(a => new UsersGridDto
                    {
                        usuarioId = a.UsuarioId,
                        nombre = a.Nombre,
                        correo = a.Correo,
                        tipoUsuario = a.TipoUsuario
                    })
                    .ToList();

                return Ok(resultado);
            }
            
            catch (Exception)
            {
                return InternalServerError(new Exception("Error al obtener la lista de usuarios."));
            }
        }

        // GET: api/profile
        [HttpGet]
        [Route("api/profile")]
        public IHttpActionResult Profile()
        {
            try
            {
                var authHeader = Request.Headers.Authorization;
                if (authHeader == null || string.IsNullOrEmpty(authHeader.Parameter))
                    return Content(System.Net.HttpStatusCode.Unauthorized, new { message = "Token faltante." });

                int? userId = JwtService.GetIdFromToken(authHeader.Parameter);
                if (!userId.HasValue)
                    return Content(System.Net.HttpStatusCode.Unauthorized, new { message = "Token inválido o expirado." });

                var user = db.Usuarios.Find(userId.Value);
                if (user == null)
                    return NotFound();

                return Ok(user);
            }
            catch (Exception)
            {
                return InternalServerError(new Exception("Error al obtener el perfil."));
            }
        }


        // Delete: api/deleteAccount
        [HttpDelete]
        [Route("api/deleteAccountbyID")]
        public IHttpActionResult DeleteAccountByID(deleteUserDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                Usuario user = ValidarAccesoDireccion();

                if (user.TipoUsuario != "Direccion")
                {
                    return Content(System.Net.HttpStatusCode.Unauthorized, new { valid = false, message = "No tiene Permisos para esta acción" });
                }
                else {
                    Usuario usuarioEliminado = db.Usuarios.Find(model.id);
                    db.Usuarios.Remove(usuarioEliminado);

                    return Ok(new
                    {
                        message = $"Se elimino el usuario {usuarioEliminado.UsuarioId}\n" +
                        $"Con el nombre de {usuarioEliminado.Nombre}"
                    });
                }
            }
            catch (Exception)
            {
                return InternalServerError(new Exception("Error al eliminar la cuenta."));
            }
        }




        // POST: api/logout
        [HttpPost]
        [Route("api/logout")]
        public IHttpActionResult Logout()
        {
            return Ok(new { message = "Sesión cerrada localmente." });
        }

        // --- Herramientas ---
        private string EncriptarSHA256(string texto)
        {
            if (string.IsNullOrEmpty(texto)) return string.Empty;

            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(texto));
                StringBuilder sb = new StringBuilder();
                foreach (byte b in bytes) sb.Append(b.ToString("x2"));
                return sb.ToString();
            }
        }
        private void EnviarCorreoCodigo(string correoDestino, string codigo)
        {


            var emisor = "sgaclient87@gmail.com";
            var password = "rgwj noks skgi xjpj"; //gSGA4343

            var mensaje = new MailMessage(emisor, correoDestino)
            {
                Subject = "Código de verificación para cambio de contraseña",
                Body = $"Tu código de seguridad es: {codigo}. Expira en 15 minutos."
            };

            using (var cliente = new SmtpClient("smtp.gmail.com", 587))
            {
                cliente.EnableSsl = true;
                cliente.Credentials = new NetworkCredential(emisor, password);
                cliente.Send(mensaje);
            }
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing) db.Dispose();
            base.Dispose(disposing);
        }
    }

    
}