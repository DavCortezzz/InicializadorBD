using System;
using System.Linq;
using System.Web.Http;
using System.Security.Cryptography;
using System.Text;
using System.Data.Entity.Infrastructure;
using Escuela.Models;

namespace Escuela.Api.Controllers
{
    public class AuthController : ApiController
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
                if (db.Usuarios.Any(u => u.Correo == model.Correo))
                    return Content(System.Net.HttpStatusCode.Conflict, new { message = "El correo electrónico ya está registrado." });

                var user = new Usuario
                {
                    Nombre = model.Nombre,
                    Correo = model.Correo,
                    Contrasena = EncriptarSHA256(model.Contrasena),
                    TipoUsuario = model.TipoUsuario
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
                
                return InternalServerError(new Exception("Ocurrió un error inesperado durante el registro."));
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
                string hash = EncriptarSHA256(model.Contrasena);
                var user = db.Usuarios.FirstOrDefault(u => u.Correo == model.Correo && u.Contrasena == hash);

                if (user == null)
                    return Content(System.Net.HttpStatusCode.Unauthorized, new { message = "Credenciales incorrectas." });

                var token = JwtService.CreateAccessToken(user.UsuarioId);

                return Ok(new
                {
                    id = user.UsuarioId,
                    name = user.Nombre,
                    email = user.Correo,
                    tipoUsuario = user.TipoUsuario,
                    token = token
                });
            }
            catch (Exception)
            {
                return InternalServerError(new Exception("Error al procesar el inicio de sesión."));
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

                return Ok(new { id = user.UsuarioId, name = user.Nombre, correo = user.Correo, tipoUsuario=user.TipoUsuario });
            }
            catch (Exception)
            {
                return InternalServerError(new Exception("Error al obtener el perfil."));
            }
        }

        [HttpGet]
        [Route("api/verify")]
        public IHttpActionResult Verify()
        {
            try
            {
                var authHeader = Request.Headers.Authorization;

                if (authHeader == null || string.IsNullOrEmpty(authHeader.Parameter))
                    return Content(System.Net.HttpStatusCode.Unauthorized, new { valid = false, message = "No se proporcionó un token." });

                int? userId = JwtService.GetIdFromToken(authHeader.Parameter);

                if (!userId.HasValue)
                    return Content(System.Net.HttpStatusCode.Unauthorized, new { valid = false, message = "Token inválido o expirado." });

               
                if (!db.Usuarios.Any(u => u.UsuarioId == userId.Value))
                    return Content(System.Net.HttpStatusCode.NotFound, new { valid = false, message = "Usuario ya no existe." });

                return Ok(new { valid = true, userId = userId.Value });
            }
            catch (Exception)
            {
                return InternalServerError(new Exception("Error al verificar la autenticidad del token."));
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