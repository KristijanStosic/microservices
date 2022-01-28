using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;
using System;
using Microsoft.Extensions.Logging;

namespace OvlascenoLiceService.ServiceCalls
{
    /// <summary>
    /// Genericka klasa za komunikaciju sa drugim servisima
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ServiceCall<T> : IServiceCall<T>
    {
        private readonly ILoggerService _loggerService;

        /// <summary>
        /// Konstruktor klase service call
        /// </summary>
        /// <param name="loggerService"></param>
        public ServiceCall(ILoggerService loggerService)
        {
            _loggerService = loggerService;
        }

        /// <summary>
        /// Metoda za slanje get zahteva
        /// </summary>
        /// <param name="url">Url putanja ka drugom servisu</param>
        /// <returns></returns>
        public async Task<T> SendGetRequestAsync(string url)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, url);
                request.Headers.Add("Accept", "application/json");

                var response = await httpClient.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    if (string.IsNullOrEmpty(content))
                    {
                        return default;
                    }

                    await _loggerService.Log(LogLevel.Information, "SendGetRequestAsync", "Komunikacija sa drugim servisom je uspešna.");

                    return JsonConvert.DeserializeObject<T>(content);
                }
                var ex = new Exception("Desio se problem pri komunikaciji sa drugim mikroservisom");
                await _loggerService.Log(LogLevel.Error, "SendGetRequestAsync", "Greška prilikom komunikacije sa drugim servisom.", ex);
                throw ex;
            }
        }
    }
}