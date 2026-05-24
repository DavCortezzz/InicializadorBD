using API_Escuela.Models;
using Escuela.Models;
using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Http;

namespace API_Escuela.Controllers
{
    public class AsistenciasController : BaseController
    {
        // POST: api/asistenciasCrear
        [HttpPost]
        [Route("api/asistenciasCrear")]
        public IHttpActionResult CrearAsistenciaGrupal(CrearAsistenciasDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                Usuario user = ValidarAccesoDireccion();

                if (user.TipoUsuario != "Profesor")
                {
                    return Content(System.Net.HttpStatusCode.Unauthorized, new { valid = false, message = "Acceso denegado. Solo los profesores pueden registrar asistencias." });
                }

                var grupo = db.Grupos.FirstOrDefault(g => g.Grado == model.grupoGrado && g.UsuarioId == user.UsuarioId);

                if (grupo == null)
                {
                    return Content(System.Net.HttpStatusCode.BadRequest, new { message = "El grupo especificado no existe o usted no es el profesor encargado del mismo." });
                }

                var asistenciasExistentes = db.Asistencias
                     .Where(a => a.Alumno.GrupoId == grupo.GrupoId && DbFunctions.TruncateTime(a.Fecha) == DbFunctions.TruncateTime(model.fecha))
                     .ToList();

                if (asistenciasExistentes.Any())
                {
                    return Content(System.Net.HttpStatusCode.Conflict, new { message = "Ya se ha generado la asistencia para este grupo en la fecha seleccionada." });
                }

                var alumnosGrupo = db.Alumnos.Where(a => a.GrupoId == grupo.GrupoId).ToList();

                if (!alumnosGrupo.Any())
                {
                    return Content(System.Net.HttpStatusCode.NotFound, new { message = "El grupo seleccionado no tiene alumnos inscritos." });
                }

                foreach (var alumno in alumnosGrupo)
                {
                    var nuevaAsistencia = new Asistencia
                    {
                        IdCURP = alumno.idCURP,
                        Fecha = model.fecha.Date, 
                        Estado = "Atendio"
                    };
                    db.Asistencias.Add(nuevaAsistencia);
                }

                db.SaveChanges();
                return Ok(new { message = $"Lista de asistencia generada exitosamente para el grupo {grupo.Grado} ({alumnosGrupo.Count} alumnos)." });
            }
            catch (Exception ex)
            {
                var errorReal = ex.InnerException?.Message ?? ex.Message;
                return InternalServerError(new Exception("Error al procesar asistencia: " + errorReal));
            }
        }

        // PUT: api/asistenciasModificar
        [HttpPut]
        [Route("api/asistenciasModificar")]
        public IHttpActionResult ModificarAsistenciaGrupal(ModificarAsistenciasDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                Usuario user = ValidarAccesoDireccion();
                if (user.TipoUsuario != "Profesor")
                {
                    return Content(System.Net.HttpStatusCode.Unauthorized, new { message = "Solo los profesores pueden modificar asistencias." });
                }

                var grupo = db.Grupos.FirstOrDefault(g => g.Grado == model.grupoGrado && g.UsuarioId == user.UsuarioId);
                if (grupo == null)
                {
                    return Content(System.Net.HttpStatusCode.BadRequest, new { message = "El grupo no existe o usted no es el profesor encargado." });
                }

                var fechaModificar = model.fecha.Date;

                var asistenciasExistentes = db.Asistencias
                    .Where(a => a.Alumno.GrupoId == grupo.GrupoId && DbFunctions.TruncateTime(a.Fecha) == fechaModificar)
                    .ToList();

                if (!asistenciasExistentes.Any())
                {
                    return Content(System.Net.HttpStatusCode.NotFound, new { message = "No existe un registro de asistencia previo en esta fecha para modificar." });
                }

                int modificados = 0;
                foreach (var itemDto in model.AlumnosAsistencia)
                {
                    var asistenciaAlumno = asistenciasExistentes.FirstOrDefault(a => a.IdCURP == itemDto.idCURP);
                    if (asistenciaAlumno != null)
                    {
                        asistenciaAlumno.Estado = itemDto.estado; 
                        modificados++;
                    }
                }

                db.SaveChanges();
                return Ok(new { message = $"Se actualizaron con éxito {modificados} registros de asistencia." });
            }
            catch (Exception ex)
            {
                return InternalServerError(new Exception("Error al modificar asistencias: " + (ex.InnerException?.Message ?? ex.Message)));
            }
        }

        // GET: api/asistenciasObtenenerLista
        [HttpGet]
        [Route("api/asistenciasObtenerLista")]
        public IHttpActionResult ObtenerListaAsistencia([FromUri]ConsultarAsistenciaDto model)
        {
            if (model == null || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                Usuario user = ValidarAccesoDireccion();
                if (user.TipoUsuario != "Profesor")
                {
                    return Content(System.Net.HttpStatusCode.Unauthorized, new { message = "Solo los profesores pueden consultar la lista de asistencia." });
                }

                // 2. Validar que el grupo exista y le pertenezca a este profesor usando el modelo 'filtro'
                var grupo = db.Grupos.FirstOrDefault(g => g.Grado == model.grupoGrado && g.UsuarioId == user.UsuarioId);
                if (grupo == null)
                {
                    return Content(System.Net.HttpStatusCode.BadRequest, new { message = "El grupo no existe o usted no es el profesor encargado del mismo." });
                }

                var fechaBusqueda = model.fecha.Date;

                // 3. Traer los alumnos inscritos en este grupo
                var alumnos = db.Alumnos.Where(a => a.GrupoId == grupo.GrupoId).ToList();
                if (!alumnos.Any())
                {
                    return Content(System.Net.HttpStatusCode.NotFound, new { message = "Este grupo no tiene alumnos inscritos actualmente." });
                }

                // 4. Traer las asistencias ya registradas (si existen) en la fecha indicada
                var asistenciasExistentes = db.Asistencias
                    .Where(a => a.Alumno.GrupoId == grupo.GrupoId && DbFunctions.TruncateTime(a.Fecha) == fechaBusqueda)
                    .ToList();

                // 5. Mapear y cruzar los datos usando tu AlumnoAsistenciaGridVM
                var listaResultado = alumnos.Select(alumno => {
                    var asistenciaHoy = asistenciasExistentes.FirstOrDefault(asist => asist.IdCURP == alumno.idCURP);

                    return new AlumnoAsistenciaGridVM
                    {
                        idCURP = alumno.idCURP,
                        nombreAlumno = $"{alumno.Nombre} {alumno.PrimerApellido} {alumno.SegundoApellido}".Trim(),
                        estado = asistenciaHoy != null ? asistenciaHoy.Estado : "Atendio" // Estado por defecto si es nueva
                    };
                }).ToList();

                // 6. Retornamos la respuesta estructurada
                return Ok(new
                {
                    grupo = grupo.Grado,
                    fecha = fechaBusqueda.ToString("yyyy-MM-dd"),
                    yaExisteRegistro = asistenciasExistentes.Any(),
                    alumnos = listaResultado
                });
            }
            catch (Exception ex)
            {
                var errorReal = ex.InnerException?.Message ?? ex.Message;
                return InternalServerError(new Exception("Error al cargar la lista de asistencia: " + errorReal));
            }
        }
    }

    
}