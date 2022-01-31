using JavnoNadmetanjeService.Models.Exceptions;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;

namespace JavnoNadmetanjeService.ServiceCalls
{
    public class ServiceCall<T> : IServiceCall<T>
    {
        private readonly ILoggerService _loggerService;

        public ServiceCall(ILoggerService loggerService)
        {
            _loggerService = loggerService;
        }

        public async Task<T> SendGetRequestAsync(string url)
        {
            using var httpClient = new HttpClient();

            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Add("Accept", "application/json");

            var response = await httpClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                if (string.IsNullOrEmpty(content))
                {
                    return default;
                }

                return JsonConvert.DeserializeObject<T>(content);
            }

            var ex = new ServiceCallException("Desio se problem pri komunikaciji sa drugim mikroservisom");
            await _loggerService.Log(LogLevel.Error, "SendGetRequestAsync", $"Greška prilikom komunikacije sa drugim servisom iz servisa Javno Nadmetanje. Ciljani url: {url}", ex);
            throw ex;
        }
    }
}
