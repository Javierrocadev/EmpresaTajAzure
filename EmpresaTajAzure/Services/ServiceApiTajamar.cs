using EmpresaTajAzure.Models;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Data;

namespace EmpresaTajAzure.Services
{
    public class ServiceApiTajamar
    {
        private string UrlApiTajamar;
        private MediaTypeWithQualityHeaderValue Header;

        public ServiceApiTajamar(IConfiguration configuration)
        {
            this.UrlApiTajamar =
                configuration.GetValue<string>("ApiUrls:ApiTajamar");
            this.Header =
                new MediaTypeWithQualityHeaderValue("application/json");
        }

        public async Task<string> GetTokenAsync(string username
            , int password)
        {
            using (HttpClient client = new HttpClient())
            {
                string request = "api/auth/login";
                client.BaseAddress = new Uri(this.UrlApiTajamar);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);
                LoginModel model = new LoginModel
                {
                    UserName = username,
                    Password = password
                };
                string jsonData = JsonConvert.SerializeObject(model);
                StringContent content =
                    new StringContent(jsonData, Encoding.UTF8,
                    "application/json");
                HttpResponseMessage response = await
                    client.PostAsync(request, content);
                if (response.IsSuccessStatusCode)
                {
                    string data = await response.Content.ReadAsStringAsync();
                    JObject keys = JObject.Parse(data);
                    string token = keys.GetValue("response").ToString();
                    return token;
                }
                else
                {
                    return null;
                }
            }
        }

        private async Task<T> CallApiAsync<T>(string request)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(this.UrlApiTajamar);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);
                HttpResponseMessage response =
                    await client.GetAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    T data = await response.Content.ReadAsAsync<T>();
                    return data;
                }
                else
                {
                    return default(T);
                }
            }
        }

        //TENDREMOS UN METODO GENERICO QUE RECIBIRA EL REQUEST 
        //Y EL TOKEN
        private async Task<T> CallApiAsync<T>
            (string request, string token)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(this.UrlApiTajamar);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);
                client.DefaultRequestHeaders.Add
                    ("Authorization", "bearer " + token);
                HttpResponseMessage response =
                    await client.GetAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    T data = await response.Content.ReadAsAsync<T>();
                    return data;
                }
                else
                {
                    return default(T);
                }
            }
        }

        //----------------------------USUARIOS---------------------------

        public async Task<List<UsuarioEmpresa>> GetUsuariosAsync(string token)
        {
            string request = "api/usuarios";
            List<UsuarioEmpresa> usuarios = await
                this.CallApiAsync<List<UsuarioEmpresa>>(request, token);
            return usuarios;
        }

        public async Task<List<UsuarioEmpresa>> FindUsuariosPorEmpresa(int idEmpresa)
        {
            string request = "api/usuarios/usuariosporempresa/"+ idEmpresa;
            List<UsuarioEmpresa> usuarios = await
                this.CallApiAsync<List<UsuarioEmpresa>>(request);
            return usuarios;
        }
        

        public async Task<UsuarioEmpresa> FindUsuarioEmailAsync
            (string email)
        {
            string request = "api/Usuarios/email/" + email;
            //string request = "api/Usuarios/email/javier.roca%40tajamar365.com";
            UsuarioEmpresa usuario = await
                this.CallApiAsync<UsuarioEmpresa>(request);
            return usuario;
        }

        public async Task InsertusuarioAsync(int idUsuario, int? idClase, string nombre, string role, string linkedin, string email, int? emp1, int? emp2, int? emp3, int? emp4, int? emp5, int? emp6)
        {
            using (HttpClient client = new HttpClient())
            {
                string request = "api/usuarios";
                client.BaseAddress = new Uri(this.UrlApiTajamar);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);
                //INSTANCIAMOS NUESTRO MODEL
                UsuarioEmpresa usuario = new UsuarioEmpresa();
                usuario.IdUsuario = 0;
                usuario.IdClase = idClase;
                usuario.Nombre = nombre;
                usuario.Role = role;
                usuario.Linkedin = linkedin;
                usuario.Email = email;
                usuario.Emp_1Id = emp1;
                usuario.Emp_2Id = emp2;
                usuario.Emp_3Id = emp3;
                usuario.Emp_4Id = emp4;
                usuario.Emp_5Id = emp5;
                usuario.Emp_6Id = emp6;

                //CONVERTIMOS NUESTRO MODEL A JSON
                string json = JsonConvert.SerializeObject(usuario);
                //PARA ENVIAR DATOS (data) AL SERVICIO DEBEMOS 
                //UTILIZAR LA CLASE StringContent QUE NOS PEDIRA
                //LOS DATOS, SU ENCODING Y EL TIPO DE FORMATO
                StringContent content =
                    new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response =
                    await client.PostAsync(request, content);
            }
        }

        //METODO PROTEGIDO
        //public async Task<UsuarioEmpresa> FindUsuarioAsync
        //    (int idUsuario, string token)
        //{
        //    string request = "api/usuarios/" + idUsuario;
        //    UsuarioEmpresa usuario = await
        //        this.CallApiAsync<UsuarioEmpresa>(request, token);
        //    return usuario;
        //}

        public async Task<UsuarioEmpresa> FindUsuarioAsync(int idUsuario)
        {
            string request = "api/usuarios/" + idUsuario;
            UsuarioEmpresa usuario = await
                this.CallApiAsync<UsuarioEmpresa>(request);
            return usuario;
        }

        public async Task<UsuarioEmpresaConNombre> FindUsuarioNombreEmpresasAsync(int idUsuario)
        {
            string request = "api/usuarios/perfil/" + idUsuario;
            UsuarioEmpresaConNombre usuario = await
                this.CallApiAsync<UsuarioEmpresaConNombre>(request);
            return usuario;
        }

        public async Task UpdateUsuarioAsync(int idUsuario, int idClase, string nombre, string role, string linkedin, string email, int? idempresa1, int? idempresa2, int? idempresa3, int? idempresa4, int? idempresa5, int? idempresa6)
        {
            using (HttpClient client = new HttpClient())
            {
                string request = "api/usuarios";
                client.BaseAddress = new Uri(this.UrlApiTajamar);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);
                UsuarioEmpresa usuario = new UsuarioEmpresa();
                usuario.IdUsuario = idUsuario;
                usuario.IdClase = idClase;
                usuario.Nombre = nombre;
                usuario.Role = role;
                usuario.Linkedin = linkedin;
                usuario.Email = email;
                usuario.Emp_1Id = idempresa1;
                usuario.Emp_2Id = idempresa2;
                usuario.Emp_3Id = idempresa3;
                usuario.Emp_4Id = idempresa4;
                usuario.Emp_5Id = idempresa5;
                usuario.Emp_6Id = idempresa6;
                string json = JsonConvert.SerializeObject(usuario);
                StringContent content =
                    new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response =
                    await client.PutAsync(request, content);
            }
        }



        //----------------------------EMPRESAS---------------------------

        //public async Task<List<Empresa>> GetEmpresasAsync()
        //{
        //    string request = "api/Empresas";
        //    List<Empresa> empresas = await
        //        this.CallApiAsync<List<Empresa>>(request);
        //    return empresas;
        //}


        public async Task<List<Empresa>> GetEmpresasAsync(string token)
        {
            string request = "api/Empresas";
            List<Empresa> empresas = await
                this.CallApiAsync<List<Empresa>>(request, token);
            return empresas;
        }
        //METODO PROTEGIDO
        //public async Task<UsuarioEmpresa> FindUsuarioAsync
        //    (int idUsuario, string token)
        //{
        //    string request = "api/usuarios/" + idUsuario;
        //    UsuarioEmpresa usuario = await
        //        this.CallApiAsync<UsuarioEmpresa>(request, token);
        //    return usuario;
        //}

        public async Task<Empresa> GetEmpresaById(int idempresa)
        {
            string request = "api/Empresas/" + idempresa;
            //string request = "api/Usuarios/email/javier.roca%40tajamar365.com";
            Empresa empresa = await
                this.CallApiAsync<Empresa>(request);
            return empresa;
        }
        public async Task InsertEmpresaAsync(int idEmpresa, string nombre, string linkedin, string imagen, int? plazas, int? plazasDisponibles)
        {
            using (HttpClient client = new HttpClient())
            {
                string request = "api/empresas";
                client.BaseAddress = new Uri(this.UrlApiTajamar);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);
                //INSTANCIAMOS NUESTRO MODEL
                Empresa empresa = new Empresa();
                empresa.IdEmpresa = 0;
                empresa.Nombre = nombre;
                empresa.Linkedin = linkedin;
                empresa.Imagen = imagen;
                empresa.Plazas = plazas;
                empresa.PlazasDisponibles = plazasDisponibles;
                //CONVERTIMOS NUESTRO MODEL A JSON
                string json = JsonConvert.SerializeObject(empresa);
                //PARA ENVIAR DATOS (data) AL SERVICIO DEBEMOS 
                //UTILIZAR LA CLASE StringContent QUE NOS PEDIRA
                //LOS DATOS, SU ENCODING Y EL TIPO DE FORMATO
                StringContent content =
                    new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response =
                    await client.PostAsync(request, content);
            }
        }




        //InsertarEmpresasEnUsuario

        //----------------------------ENTREVISTAS------------------


        public async Task<List<EntrevistaAlumno>> FindEntrevistasUsuarioAsync(int idUsuario)
        {
            string request = "api/Entrevistas/entrevistasPorUsuario/" + idUsuario;
           List<EntrevistaAlumno> entrevistas = await
                this.CallApiAsync<List<EntrevistaAlumno>>(request);
            return entrevistas;
        }

        public async Task InsertEntrevistaAsync
    (int identEntrevista, int idAlumno, int idEmpresa, DateTime fechaEntrevista, string estado)
        {
            using (HttpClient client = new HttpClient())
            {
                string request = "api/entrevistas";
                client.BaseAddress = new Uri(this.UrlApiTajamar);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);
                //INSTANCIAMOS NUESTRO MODEL
                EntrevistaAlumno entrevista = new EntrevistaAlumno();
                entrevista.IdentEntrevista = identEntrevista;
                entrevista.IdAlumno = idAlumno;
                entrevista.IdEmpresa = idEmpresa;
                entrevista.FechaEntrevista = fechaEntrevista;
                entrevista.Estado = estado;
                //CONVERTIMOS NUESTRO MODEL A JSON
                string json = JsonConvert.SerializeObject(entrevista);
                //PARA ENVIAR DATOS (data) AL SERVICIO DEBEMOS 
                //UTILIZAR LA CLASE StringContent QUE NOS PEDIRA
                //LOS DATOS, SU ENCODING Y EL TIPO DE FORMATO
                StringContent content =
                    new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response =
                    await client.PostAsync(request, content);
            }
        }









        

    }
}

