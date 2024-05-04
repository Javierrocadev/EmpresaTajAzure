using Newtonsoft.Json;
using System.Configuration;
using System.Net.Http.Headers;
using System.Text;

using Microsoft.Extensions.Configuration;

namespace EmpresaTajAzure.Services
{
    public class ServiceLogicApps
    {
        private readonly MediaTypeWithQualityHeaderValue _header;
        private readonly string _urlLogicApp;

        public ServiceLogicApps(string urlLogicApp)
        {
            _urlLogicApp = urlLogicApp;
            _header = new MediaTypeWithQualityHeaderValue("application/json");
        }

        public async Task SendEmailAsync(string email, string asunto, string mensaje)
        {
            var model = new
            {
                email = email,
                asunto = asunto,
                mensaje = mensaje
            };
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(_header);
                string json = JsonConvert.SerializeObject(model);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(_urlLogicApp, content);
            }
        }
    }
}
