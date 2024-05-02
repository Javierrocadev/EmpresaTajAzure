using EmpresaTajAzure.Data;
using EmpresaTajAzure.Models;
using EmpresaTajAzure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmpresaTajAzure.Controllers
{
    public class UsuariosController : Controller
    {

        private ServiceApiTajamar service;
        private ApplicationContext context;

        public UsuariosController(ServiceApiTajamar service, ApplicationContext context)
        {
            this.service = service;
            this.context = context;
        }

        public async Task<IActionResult> ListaUsuarios()
        {
            List<UsuarioEmpresa> usuarios = await
                this.service.GetUsuariosAsync();
            return View(usuarios);
        }


        [Authorize]
        public IActionResult Perfil()
        {
            return View();
        }

        [Authorize]
        public async Task<IActionResult> Registro()
        {
            if (User.Identity.IsAuthenticated)
            {
                string userEmail = User.Identity.Name;
                UsuarioEmpresa usuario = await this.service.FindUsuarioEmailAsync(userEmail);

                if (usuario != null)
                {
                    string token = await this.service.GetTokenAsync(userEmail, usuario.IdUsuario);
                    if (token == null)
                    {
                        ViewData["MENSAJE"] = "Usuario/Password incorrectos";
                    }
                    else
                    {
                        ViewData["MENSAJE"] = "Ya tienes tu token";
                        HttpContext.Session.SetString("TOKEN", token);
                        return RedirectToAction("Perfil", "Usuarios");
                    }
                }

            }
            return View();
        }

        //[HttpPost]
        //public async Task<IActionResult> Registro(string email, int idClase, string nombre, string linkedin)
        //{




        //    return View();
        //}






    }
}
