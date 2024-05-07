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
            _urlLogicApp = "https://prod-12.westeurope.logic.azure.com:443/workflows/a8e867bd5a6f409a9c8bccab7f074347/triggers/When_a_HTTP_request_is_received/paths/invoke?api-version=2016-10-01&sp=%2Ftriggers%2FWhen_a_HTTP_request_is_received%2Frun&sv=1.0&sig=P2u5BovXF3AZCGl7QNwsWOcAPcnoYSOPXsRZFEzC_y8";
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
