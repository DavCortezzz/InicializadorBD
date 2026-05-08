using System;
using System.Linq;
using System.Web.Http;
using System.Security.Cryptography;
using System.Text;
using System.Data.Entity.Infrastructure;
using API_Escuela.Models;
using Escuela.Models;
using System.Web.Services.Description;
using API_Escuela.Controllers;

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
                if (db.Usuarios.Any(u => u.Correo == model.correo))
                    return Content(System.Net.HttpStatusCode.Conflict, new { message = "El correo electrónico ya está registrado." });

                var user = new Usuario
                {
                    Nombre = model.nombre,
                    Correo = model.correo,
                    Contrasena = EncriptarSHA256(model.contrasena),
                    TipoUsuario = model.tipoUsuario
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
                
                return InternalServerError(new Exception("Ocurrió un error inesperado durante el registro. "+ex));
            }
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
                    return Content(System.Net.HttpStatusCode.Unauthorized, new { message = "Credenciales incorrectas." });

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

        // GET: api/users
        [HttpGet]
        [Route("api/users")]
        public IHttpActionResult GetUsers()
        {
            try
            {
                Usuario user = ValidarAccesoDireccion();
                if (user.TipoUsuario != "Direccion")
                    return Content(System.Net.HttpStatusCode.Unauthorized, new { message = "No tiene permisos para esta acción." });

                var users = db.Usuarios.Select(u => new { id = u.UsuarioId, name = u.Nombre, correo = u.Correo, tipoUsuario=u.TipoUsuario }).ToList();
                return Ok(users);
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

                return Ok(new { id = user.UsuarioId, name = user.Nombre, correo = user.Correo, tipoUsuario=user.TipoUsuario});
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

        protected override void Dispose(bool disposing)
        {
            if (disposing) db.Dispose();
            base.Dispose(disposing);
        }
    }

    
}