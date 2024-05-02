using EmpresaTajAzure.Models;
using EmpresaTajAzure.Services;
using Microsoft.AspNetCore.Mvc;

namespace EmpresaTajAzure.Controllers
{
    public class EmpresasController : Controller
    {
        private ServiceApiTajamar service;

        public EmpresasController(ServiceApiTajamar service)
        {
            this.service = service;
        }

        public async Task<IActionResult> Empresas()
        {
            List<Empresa> empresas = await this.service.GetEmpresasAsync();
            return View(empresas);
        }
    }
}
