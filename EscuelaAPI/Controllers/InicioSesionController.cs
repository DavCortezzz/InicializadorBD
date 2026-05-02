using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Escuela.Models;

namespace EscuelaAPI.Controllers
{
    public class InicioSesionController : ApiController
    {
        private EscuelaContext db = new EscuelaContext();
    }
}
