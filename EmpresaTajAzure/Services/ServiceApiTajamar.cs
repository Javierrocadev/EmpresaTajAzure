using EmpresaTajAzure.Models;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

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

        public async Task<List<UsuarioEmpresa>> GetUsuariosAsync()
        {
            string request = "api/usuarios";
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

        //METODO PROTEGIDO
        public async Task<UsuarioEmpresa> FindUsuarioAsync
            (int idUsuario, string token)
        {
            string request = "api/usuarios/" + idUsuario;
            UsuarioEmpresa usuario = await
                this.CallApiAsync<UsuarioEmpresa>(request, token);
            return usuario;
        }

    }
}

