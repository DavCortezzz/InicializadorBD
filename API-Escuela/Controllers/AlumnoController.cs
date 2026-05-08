using System;
using System.Linq;
using System.Web.Http;
using System.Security.Cryptography;
using System.Text;
using System.Data.Entity.Infrastructure;
using API_Escuela.Models;
using Escuela.Models;
using System.Web.Services.Description;
using System.Data.Entity.Migrations;

namespace API_Escuela.Controllers
{
    public class AlumnoController : BaseController
    {
        private EscuelaContext db = new EscuelaContext();

        // POST: api/alumnos
        [HttpPost]
        [Route("api/alumnos")]
        public IHttpActionResult RegisterAlumno(RegistroAlumnosDto model)
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
                else
                {
                    if (db.Alumnos.Any(a => a.idCURP == model.idCURP))
                        return Content(System.Net.HttpStatusCode.Conflict, new { message = "El CURP del alumno ya está registrado." });

                    var Alumno = new Alumno
                    {
                        Nombre = model.nombre,
                        idCURP = model.idCURP,
                        FechaNacimiento = model.fechaNacimiento,
                        Tutor = model.tutor,
                        TelefonoTutor = model.telefonoTutor,
                        GrupoId = model.grupoId
                    };

                    db.Alumnos.Add(Alumno);
                    db.SaveChanges();

                    return Ok(new
                    {
                        message = $"Se ha creado el alumno {Alumno.idCURP}\n" +
                        $"Con el nombre de {Alumno.Nombre}",
                        Alumno
                    });
                }
            }
            catch (Exception)
            {
                return InternalServerError(new Exception("Error al registrar el alumno."));
            }
        }

        // POST: api/alumnos
        [HttpPost]
        [Route("api/alumnos/:idCURP")]
        public IHttpActionResult UpdateAlumno(ModificarAlumnosDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {

                Usuario user = ValidarAccesoDireccion();

                if (user == null || user.TipoUsuario != "Direccion")
                    return Content(System.Net.HttpStatusCode.Unauthorized, new { valid = false, message = "No tiene permisos para esta acción." });

                var alumnoAModificar = db.Alumnos.FirstOrDefault(a => a.idCURP == model.idCURP);
                if (alumnoAModificar == null)
                    return Content(System.Net.HttpStatusCode.NotFound, new { message = "El alumno con la CURP proporcionada no existe." });

                if (!string.IsNullOrWhiteSpace(model.nuevaIdCURP) && model.nuevaIdCURP != model.idCURP)
                {
                    if (db.Alumnos.Any(a => a.idCURP == model.nuevaIdCURP))
                        return Content(System.Net.HttpStatusCode.Conflict, new { message = "La nueva CURP ya está registrada por otro alumno." });

                    alumnoAModificar.idCURP = model.nuevaIdCURP;
                }

                if (!string.IsNullOrWhiteSpace(model.nombre))
                    alumnoAModificar.Nombre = model.nombre;

                if (model.fechaNacimiento != null)
                    alumnoAModificar.FechaNacimiento = model.fechaNacimiento;

                if (!string.IsNullOrWhiteSpace(model.tutor))
                    alumnoAModificar.Tutor = model.tutor;

                if (!string.IsNullOrWhiteSpace(model.telefonoTutor))
                    alumnoAModificar.TelefonoTutor = model.telefonoTutor;

                if (model.grupoId != null)
                    alumnoAModificar.GrupoId = model.grupoId;

                db.SaveChanges();

                return Ok(new
                {
                    message = "Alumno actualizado correctamente.",
                    alumno = alumnoAModificar
                });
            }
            catch (Exception ex)
            {
                return InternalServerError(new Exception("Ocurrió un error inesperado al procesar la solicitud."));
            }
        }

        //Get: api/alumnos
        [HttpGet]
        [Route("api/alumnos")]
        public IHttpActionResult GetAlumnos() {
            try
            {
                Usuario user = ValidarAccesoDireccion();
                if (user.TipoUsuario != "Direccion")
                    return Content(System.Net.HttpStatusCode.Unauthorized, new { message = "No tiene permisos para esta acción." });

                var Alumnos = db.Alumnos.Select(a => new {a.idCURP ,  a.Nombre,a.FechaNacimiento,a.Grupo,a.TelefonoTutor,a.Tutor }).ToList();
                return Ok(Alumnos);
            }
            catch (Exception)
            {
                return InternalServerError(new Exception("Error al obtener la lista de usuarios."));
            }
        }

        //Get: api/alumnos/:grupoId
        [HttpGet]
        [Route("api/alumnos/:grupoId")]
        public IHttpActionResult GetAlumnosByGroupId(ObtenerAlumnosPorGrupoDto model ) {

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                Usuario user = ValidarAccesoDireccion();

                var grupo = db.Grupos.Find(model.grupoId);
                if (user == null)
                    return NotFound();
                if (grupo == null)
                    return NotFound();

                var Alumnos = db.Alumnos.Select(a => new { a.idCURP, a.Nombre, a.FechaNacimiento, a.Grupo, a.TelefonoTutor, a.Tutor }).Where(a => a.Grupo.GrupoId == model.grupoId ).ToList();
                return Ok(Alumnos);
            }
            catch (Exception)
            {
                return InternalServerError(new Exception("Error al obtener la lista de usuarios."));
            }
        }

        //Get: api/alumnos/:idCURP
        [HttpGet]
        [Route("api/alumnos/:idCURP")]
        public IHttpActionResult GetAlumnosByIdCURP(ObtenerAlumnoPorIdCURP model)
        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                Usuario user = ValidarAccesoDireccion();

                var alumno = db.Alumnos.Find(model.idCURP);
                if (alumno == null)
                    return NotFound();

                return Ok(alumno);
            }
            catch (Exception)
            {
                return InternalServerError(new Exception("Error al obtener la lista de usuarios."));
            }
        }
        //Delete: api/alumnos
        [HttpDelete]
        [Route("api/alumnos")]
        public IHttpActionResult DeleteAlumnoById(ObtenerAlumnoPorIdCURP model) {
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

                    Alumno alumnoAEliminar = db.Alumnos.Find(model.idCURP);
                    db.Alumnos.Remove(alumnoAEliminar);

                    return Ok(new
                    {
                        message = $"Se elimino el usuario {alumnoAEliminar.idCURP}\n" +
                        $"Con el nombre de {alumnoAEliminar.Nombre}"
                    });
                }
            }
            catch (Exception)
            {
                return InternalServerError(new Exception("Error al eliminar la cuenta."));
            }
        }


    }
}
