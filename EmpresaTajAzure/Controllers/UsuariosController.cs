using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmpresaTajAzure.Controllers
{
    public class UsuariosController : Controller
    {
        [Authorize]
        public IActionResult Perfil()
        {
            return View();
        }
    }
}
