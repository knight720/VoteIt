using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace VoteIt.Services
{
    public class NotifyService
    {
        private readonly IConfiguration _configuration;
        private readonly string _webhookURL;
        private readonly IHttpClientFactory _httpClientFactory;

        public NotifyService(IConfiguration configuration, IHttpClientFactory httpClientFactory)
        {
            this._configuration = configuration;
            this._webhookURL = this._configuration["Slack:Webhook:URL"];
            this._httpClientFactory = httpClientFactory;
        }

        public async Task<string> Send(string message)
        {
            var httpClient = this._httpClientFactory.CreateClient();
            var data = new { text = message };

            var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");

            var request = new HttpRequestMessage(HttpMethod.Post, this._webhookURL);
            request.Content = content;

            var response = await httpClient.SendAsync(request);

            return response.StatusCode.ToString();
        }
    }
}