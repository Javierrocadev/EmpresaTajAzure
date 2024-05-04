using EmpresaTajAzure.Services;
using Microsoft.AspNetCore.Mvc;

namespace EmpresaTajAzure.Controllers
{
    public class LogicAppsController : Controller
    {
        private ServiceLogicApps service;

        public LogicAppsController(ServiceLogicApps service)
        {
            this.service = service;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index
            (string email, string asunto, string mensaje)
        {
            await this.service.SendEmailAsync(email, asunto, mensaje);
            ViewData["MENSAJE"] = "Procesando Email Logic Apps";
            //return View();
            return RedirectToAction("EmpresasAlumnos", "Empresas");
        }
    }

}
