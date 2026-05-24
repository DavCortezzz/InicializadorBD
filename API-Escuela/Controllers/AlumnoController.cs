using API_Escuela.Models;
using Escuela.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace API_Escuela.Controllers
{
    public class AlumnoController : BaseController
    {
        // POST: api/alumnosRegistrar
        [HttpPost]
        [Route("api/alumnosRegistrar")]
        public IHttpActionResult RegisterAlumno(RegistroAlumnosDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                Usuario user = ValidarAccesoDireccion();

                if (user.TipoUsuario != "Direccion")
                    return Content(System.Net.HttpStatusCode.Unauthorized, new { valid = false, message = "No tiene Permisos para esta acción" });

                if (db.Alumnos.Any(a => a.idCURP == model.idCURP))
                    return Content(System.Net.HttpStatusCode.Conflict, new { message = "El CURP del alumno ya está registrado." });

                var grupo = db.Grupos.FirstOrDefault(g => g.Grado == model.gradoGrupo);
                if (grupo == null)
                    return Content(System.Net.HttpStatusCode.BadRequest, new { message = "El grupo especificado no existe." });

                var Alumno = new Alumno
                {
                    Nombre = model.nombre,
                    PrimerApellido = model.primerApellido,
                    SegundoApellido = model.segundoApellido,
                    idCURP = model.idCURP,
                    FechaNacimiento = model.fechaNacimiento,
                    Tutor = model.tutor,
                    PrimerApellidoTutor = model.primerApellidoTutor,
                    SegundoApellidoTutor = model.segundoApellidoTutor,
                    TelefonoTutor = model.telefonoTutor,
                    GrupoId = grupo.GrupoId,
                    Direccion = model.direccion,
                    ParentescoTutor = model.parentescoTutor
                };

                db.Alumnos.Add(Alumno);
                db.SaveChanges();

                return Ok(new { message = $"Se ha creado el alumno {Alumno.idCURP} con éxito.", Alumno });
            }
            catch (Exception ex)
            {
                return InternalServerError(new Exception("Error al registrar el alumno: " + ex.Message));
            }
        }

        // POST: api/alumnosModificar
        [HttpPost]
        [Route("api/alumnosModificar")]
        public IHttpActionResult UpdateAlumno(ModificarAlumnosDto model)
        {
            if (model == null || !ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                Usuario user = ValidarAccesoDireccion();
                if (user == null || user.TipoUsuario != "Direccion")
                    return Content(System.Net.HttpStatusCode.Unauthorized, new { message = "No tiene permisos." });

                var alumnoAModificar = db.Alumnos.FirstOrDefault(a => a.idCURP == model.idCURP);
                if (alumnoAModificar == null)
                    return Content(System.Net.HttpStatusCode.NotFound, new { message = "El alumno no existe." });

                if (!string.IsNullOrWhiteSpace(model.nuevaIdCURP) && model.nuevaIdCURP != model.idCURP)
                {
                    if (db.Alumnos.Any(a => a.idCURP == model.nuevaIdCURP))
                        return Content(System.Net.HttpStatusCode.Conflict, new { message = "La nueva CURP ya existe." });

                    db.Database.ExecuteSqlCommand(
                        "UPDATE dbo.Alumno SET idCURP = @nueva WHERE idCURP = @vieja",
                        new System.Data.SqlClient.SqlParameter("@nueva", model.nuevaIdCURP),
                        new System.Data.SqlClient.SqlParameter("@vieja", model.idCURP)
                    );

                    alumnoAModificar = db.Alumnos.FirstOrDefault(a => a.idCURP == model.nuevaIdCURP);
                }

                if (!string.IsNullOrWhiteSpace(model.nombre)) alumnoAModificar.Nombre = model.nombre;
                if (!string.IsNullOrWhiteSpace(model.primerApellido)) alumnoAModificar.PrimerApellido = model.primerApellido;
                if (!string.IsNullOrWhiteSpace(model.segundoApellido)) alumnoAModificar.SegundoApellido = model.segundoApellido;
                if (model.fechaNacimiento != null) alumnoAModificar.FechaNacimiento = model.fechaNacimiento;
                if (!string.IsNullOrWhiteSpace(model.tutor)) alumnoAModificar.Tutor = model.tutor;
                if (!string.IsNullOrWhiteSpace(model.telefonoTutor)) alumnoAModificar.TelefonoTutor = model.telefonoTutor;
                if (!string.IsNullOrWhiteSpace(model.direccion)) alumnoAModificar.Direccion = model.direccion;

                if (!string.IsNullOrEmpty(model.gradoGrupo))
                {
                    var grupo = db.Grupos.FirstOrDefault(g => g.Grado == model.gradoGrupo);
                    if (grupo != null) alumnoAModificar.GrupoId = grupo.GrupoId;
                }

                db.Entry(alumnoAModificar).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

                return Ok(new { message = "Alumno actualizado correctamente." });
            }
            catch (Exception ex)
            {
                var errorReal = ex.InnerException?.InnerException?.Message ?? ex.InnerException?.Message ?? ex.Message;
                return InternalServerError(new Exception("Error al guardar: " + errorReal));
            }
        }

        // POST: api/alumnos
        [HttpPost]
        [Route("api/alumnos")]
        public IHttpActionResult PostAlumnos(BuscarAlumnosByGroupIdONombreCURPDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                Usuario user = ValidarAccesoDireccion();
                if (user == null) return Unauthorized();

                var query = db.Alumnos.AsQueryable();

                if (!string.IsNullOrEmpty(model.gradoGrupo))
                    query = query.Where(a => a.Grupo.Grado == model.gradoGrupo);

                if (!string.IsNullOrEmpty(model.nombreCURP))
                {
                    string busqueda = model.nombreCURP.ToLower().Trim();
                    query = query.Where(a =>
                        (a.Nombre + " " + a.PrimerApellido + " " + a.SegundoApellido).ToLower().Contains(busqueda) ||
                        a.idCURP.ToLower().Contains(busqueda)
                    );
                }

                var resultado = query
                    .OrderBy(a => a.Nombre)
                    .Select(a => new AlumnoGridModel
                    {
                        idCURP = a.idCURP,
                        Nombre = a.Nombre + " " + a.PrimerApellido + " " + a.SegundoApellido,
                        Tutor = a.Tutor + " " + a.PrimerApellidoTutor + " " + a.SegundoApellidoTutor,
                        TelefonoTutor = a.TelefonoTutor,
                        gradoGrupo = a.Grupo.Grado
                    })
                    .ToList();

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return InternalServerError(new Exception("Error al filtrar alumnos: " + ex.Message));
            }
        }

        // GET: api/alumnos/{idCURP}
        [HttpGet]
        [Route("api/alumnos/{idCURP}")]
        public IHttpActionResult GetAlumnosByIdCURP([FromUri] ObtenerAlumnoPorIdCURP model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                ValidarAccesoDireccion();

                var alumno = db.Alumnos.Where(a => a.idCURP == model.idCURP)
                                    .Select(a => new AlumnoObtenidoModel
                                    {
                                        idCURP = a.idCURP,
                                        nombre = a.Nombre,
                                        primerApellido = a.PrimerApellido,
                                        segundoApellido = a.SegundoApellido,
                                        tutor = a.Tutor,
                                        primerApellidoTutor = a.PrimerApellidoTutor,
                                        segundoApellidoTutor = a.SegundoApellidoTutor,
                                        telefonoTutor = a.TelefonoTutor,
                                        gradoGrupo = a.Grupo.Grado,
                                        fechaNacimiento = a.FechaNacimiento,
                                        direccion = a.Direccion,
                                        parentescoTutor = a.ParentescoTutor,
                                    }).FirstOrDefault();

                if (alumno == null)
                    return NotFound();

                return Ok(alumno);
            }
            catch (Exception ex)
            {
                return InternalServerError(new Exception("Error al obtener el alumno: " + ex.Message));
            }
        }

        // DELETE: api/alumnos
        [HttpDelete]
        [Route("api/alumnos")]
        public IHttpActionResult DeleteAlumnoById(ObtenerAlumnoPorIdCURP model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                Usuario user = ValidarAccesoDireccion();
                if (user == null || user.TipoUsuario != "Direccion")
                    return Content(System.Net.HttpStatusCode.Unauthorized, new { valid = false, message = "No tiene permisos para esta acción." });

                var alumnoAEliminar = db.Alumnos.FirstOrDefault(a => a.idCURP == model.idCURP);
                if (alumnoAEliminar == null)
                    return Content(System.Net.HttpStatusCode.NotFound, new { message = "El alumno no existe." });

                db.Alumnos.Remove(alumnoAEliminar);
                db.SaveChanges();

                return Ok(new
                {
                    success = true,
                    message = $"Se eliminó correctamente al alumno con CURP: {alumnoAEliminar.idCURP}"
                });
            }
            catch (Exception ex)
            {
                var errorReal = ex.InnerException?.Message ?? ex.Message;
                return InternalServerError(new Exception("Error crítico al eliminar: " + errorReal));
            }
        }
    }
}