using KorisnikSistemaService.Models.Other;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace KorisnikSistemaService.ServiceCalls
{
    public class LoggerService : ILoggerService
    {
        private readonly IConfiguration _configuration;
        public LoggerService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<bool> Log(LogLevel level, string metoda, string poruka, Exception greska = null)
        {
            try
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    string url = _configuration["Services:LoggerService"];
                    var log = new LogModel
                    {
                        Servis = "Korisnik Sistema API",
                        Level = level,
                        Metoda = metoda,
                        Poruka = poruka,
                        Greska = greska
                    };

                    HttpContent content = new StringContent(JsonConvert.SerializeObject(log));
                    content.Headers.ContentType.MediaType = "application/json";

                    HttpResponseMessage response = httpClient.PostAsync(url, content).Result;

                    return await Task.FromResult(response.IsSuccessStatusCode);
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
