using EmpresaTajAzure.Models;
using EmpresaTajAzure.Services;
using Microsoft.AspNetCore.Mvc;

namespace EmpresaTajAzure.Controllers
{
    public class EmpresasController : Controller
    {
        private ServiceApiTajamar service;
        private ServiceStorageBlobs serviceBlobs;

        public EmpresasController(ServiceApiTajamar service, ServiceStorageBlobs serviceBlobs)
        {
            this.service = service;
            this.serviceBlobs = serviceBlobs;
        }


        public async Task<IActionResult> EmpresasAlumnos()
        {
            List<Empresa> empresas = await this.service.GetEmpresasAsync();
            List<BlobModel> blobs = await this.serviceBlobs.GetBlobsAsync("images");
            var model = new Tuple<List<Empresa>, List<BlobModel>>(empresas, blobs);
            return View(model);
            //return View(empresas);
        }





        public async Task<IActionResult> Empresas()
        {
            List<Empresa> empresas = await this.service.GetEmpresasAsync();
            List<BlobModel> blobs = await this.serviceBlobs.GetBlobsAsync("images");
            var model = new Tuple<List<Empresa>, List<BlobModel>>(empresas, blobs);
            return View(model);
            //return View(empresas);
        }

        public async Task<IActionResult> Index()
        {
            List<Empresa> empresas = await this.service.GetEmpresasAsync();
            List<BlobModel> blobs = await this.serviceBlobs.GetBlobsAsync("images");
            var model = new Tuple<List<Empresa>, List<BlobModel>>(empresas, blobs);
            return View(model);
            //return View(empresas);
        }


        public async Task<IActionResult> _EmpresasSeleccionadas(int? idempresa1, int? idempresa2, int? idempresa3, int? idempresa4, int? idempresa5, int? idempresa6)
        {
            List<Empresa> empresas = new List<Empresa>();

            if (idempresa1.HasValue)
            {
                var empresa1 = await this.service.GetEmpresaById(idempresa1.Value);
                if (empresa1 != null)
                {
                    empresas.Add(empresa1);

                }
            }

            if (idempresa2.HasValue)
            {
                var empresa2 = await this.service.GetEmpresaById(idempresa2.Value);
                if (empresa2 != null)
                {
                    empresas.Add(empresa2);

                }
            }

            if (idempresa3.HasValue)
            {
                var empresa3 = await this.service.GetEmpresaById(idempresa3.Value);
                if (empresa3 != null)
                {
                    empresas.Add(empresa3);

                }
            }

            if (idempresa4.HasValue)
            {
                var empresa4 = await this.service.GetEmpresaById(idempresa4.Value);
                if (empresa4 != null)
                {
                    empresas.Add(empresa4);

                }
            }

            if (idempresa5.HasValue)
            {
                var empresa5 = await this.service.GetEmpresaById(idempresa5.Value);
                if (empresa5 != null)
                {
                    empresas.Add(empresa5);

                }
            }

            if (idempresa6.HasValue)
            {
                var empresa6 = await this.service.GetEmpresaById(idempresa6.Value);
                if (empresa6 != null)
                {
                    empresas.Add(empresa6);

                }
            }

            return PartialView("_EmpresasSeleccionadas", empresas);
        }


        public IActionResult InsertarEmpresa()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> InsertarEmpresa(string nombre, string linkedin, IFormFile file, int plazas, int plazasDisponibles)
        {
            int idUsuario = int.Parse(HttpContext.Session.GetString("IDUSUARIO"));

            Empresa empresa = new Empresa();
            empresa.IdEmpresa = 0;
            empresa.Nombre = nombre;
            empresa.Linkedin = linkedin;
            empresa.Imagen = file.FileName;
            empresa.Plazas = plazas;
            empresa.PlazasDisponibles = plazasDisponibles;

            await this.service.InsertEmpresaAsync(empresa.IdEmpresa, empresa.Nombre,empresa.Linkedin, empresa.Imagen, empresa.Plazas,empresa.PlazasDisponibles);

            string blobName = file.FileName;
            using (Stream stream = file.OpenReadStream())
            {
                await this.serviceBlobs.UploadBlobAsync
                    ("images", blobName, stream);
            }


            return RedirectToAction("Index");
        }










    }
}
