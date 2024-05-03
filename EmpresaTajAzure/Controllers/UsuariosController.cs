using EmpresaTajAzure.Data;
using EmpresaTajAzure.Models;
using EmpresaTajAzure.Services;
using Microsoft.AspNetCore.Authentication;
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

        public async Task<IActionResult> PerfilUsuario()
        {
            int idUsuario = int.Parse(HttpContext.Session.GetString("IDUSUARIO"));

            UsuarioEmpresaConNombre usuario = await this.service.FindUsuarioNombreEmpresasAsync(idUsuario);
          
                      
            return View(usuario);
        }


        [Authorize]
        public IActionResult Perfil()
        {
            return View();
        }

        public IActionResult LogOut()
        {
            // Eliminar todas las sesiones
            HttpContext.Session.Clear();

            // Sign out de todos los esquemas de autenticación
            return SignOut(new AuthenticationProperties { RedirectUri = "/" }, "Identity.Application");
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
                        HttpContext.Session.SetString("NOMBREUSUARIO", usuario.Nombre);
                        HttpContext.Session.SetString("IDUSUARIO", usuario.IdUsuario.ToString());
                        HttpContext.Session.SetString("IDCLASE", usuario.IdClase.ToString());
                        HttpContext.Session.SetString("ROLE", usuario.Role);
                        HttpContext.Session.SetString("LINKEDIN", usuario.Linkedin);
                        HttpContext.Session.SetString("EMAIL", usuario.Email);
                        HttpContext.Session.SetString("EMPRESA1", usuario.Emp_1Id.ToString());
                        HttpContext.Session.SetString("EMPRESA2", usuario.Emp_2Id.ToString());
                        HttpContext.Session.SetString("EMPRESA3", usuario.Emp_3Id.ToString());
                        HttpContext.Session.SetString("EMPRESA4", usuario.Emp_4Id.ToString());
                        HttpContext.Session.SetString("EMPRESA5", usuario.Emp_5Id.ToString());
                        HttpContext.Session.SetString("EMPRESA6", usuario.Emp_6Id.ToString());
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


        public async Task<IActionResult> InsertarEmpresaAlumno(int? idempresa1, int? idempresa2, int? idempresa3, int? idempresa4, int? idempresa5, int? idempresa6)
        {
           
            int idUsuario = int.Parse(HttpContext.Session.GetString("IDUSUARIO"));
            int idClase = int.Parse(HttpContext.Session.GetString("IDCLASE"));
            string nombre = HttpContext.Session.GetString("NOMBREUSUARIO");
            string role = HttpContext.Session.GetString("ROLE");
            string linkedin = HttpContext.Session.GetString("LINKEDIN");
            string email = HttpContext.Session.GetString("EMAIL");
            await this.service.UpdateUsuarioAsync(idUsuario, idClase, nombre, role, linkedin, email, idempresa1, idempresa2, idempresa3, idempresa4, idempresa5, idempresa6);

            // Aquí puedes devolver una respuesta JSON u otro tipo de respuesta según tu necesidad
            return View();
        }



    }
}
