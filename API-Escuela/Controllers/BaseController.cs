using API_Escuela.Models;
using Escuela.Models;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace API_Escuela.Controllers
{
    public class BaseController : ApiController
    {
        protected EscuelaContext db = new EscuelaContext();

        protected Usuario ValidarAccesoDireccion()
        {
            var authHeader = Request.Headers.Authorization;

            if (authHeader == null || string.IsNullOrEmpty(authHeader.Parameter))
                throw new HttpResponseException(Request.CreateResponse(
                    HttpStatusCode.Unauthorized, new { valid = false, message = "No se proporcionó un token." }));

            int? userId = JwtService.GetIdFromToken(authHeader.Parameter);

            if (!userId.HasValue)
                throw new HttpResponseException(Request.CreateResponse(
                    HttpStatusCode.Unauthorized, new { valid = false, message = "Token inválido o expirado." }));

            var user = db.Usuarios.Find(userId.Value);

            if (user == null)
                throw new HttpResponseException(Request.CreateResponse(
                    HttpStatusCode.NotFound, new { valid = false, message = "Usuario ya no existe." }));

            return user;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && db != null)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}