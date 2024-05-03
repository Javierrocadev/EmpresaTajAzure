using EmpresaTajAzure.Data;
using EmpresaTajAzure.Models;
using EmpresaTajAzure.Services;
using Microsoft.AspNetCore.Mvc;

namespace EmpresaTajAzure.Controllers
{
    public class EntrevistasController : Controller
    {
        private ServiceApiTajamar service;
        private ApplicationContext context;

        public EntrevistasController(ServiceApiTajamar service, ApplicationContext context)
        {
            this.service = service;
            this.context = context;
        }

        public async Task<IActionResult> ListaEntrevistas()
        {
            int idUsuario = int.Parse(HttpContext.Session.GetString("IDUSUARIO"));
            List<EntrevistaAlumno> entrevistas = await
                this.service.FindEntrevistasUsuarioAsync(idUsuario);
            return View(entrevistas);
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult InsertarEntrevistaAlumno()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> InsertarEntrevistaAlumno(int idEmpresa, DateTime fechaEntrevista, string estado)
        {
            int idUsuario = int.Parse(HttpContext.Session.GetString("IDUSUARIO"));

            EntrevistaAlumno entrevista = new EntrevistaAlumno();
            entrevista.IdentEntrevista = 0;
            entrevista.IdAlumno = idUsuario;
            entrevista.IdEmpresa = idEmpresa;
            entrevista.FechaEntrevista = fechaEntrevista;
            entrevista.Estado = estado;

            await this.service.InsertEntrevistaAsync(entrevista.IdentEntrevista, entrevista.IdAlumno, entrevista.IdEmpresa, entrevista.FechaEntrevista, entrevista.Estado);
            return RedirectToAction("ListaEntrevistas");
        }

        //    public async Task<IActionResult> InsertarEntrevistaAlumno(int identEntrevista, int id)
        //{

        //    int idUsuario = int.Parse(HttpContext.Session.GetString("IDUSUARIO"));
        //    int idClase = int.Parse(HttpContext.Session.GetString("IDCLASE"));
        //    string nombre = HttpContext.Session.GetString("NOMBREUSUARIO");
        //    string role = HttpContext.Session.GetString("ROLE");
        //    string linkedin = HttpContext.Session.GetString("LINKEDIN");
        //    string email = HttpContext.Session.GetString("EMAIL");
        //    await this.service.UpdateUsuarioAsync(idUsuario, idClase, nombre, role, linkedin, email, idempresa1, idempresa2, idempresa3, idempresa4, idempresa5, idempresa6);

        //    // Aquí puedes devolver una respuesta JSON u otro tipo de respuesta según tu necesidad
        //    return View();
        //}


    }
}
