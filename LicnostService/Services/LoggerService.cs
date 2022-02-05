using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using LicnostService.Models.OtherModels;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace LicnostService.Services
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
            using (HttpClient httpClient = new HttpClient())
            {
                string url = _configuration["Services:LoggerService"];
                var log = new LoggerModel
                {
                    Servis = "Licnost API",
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
    }
}
