using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using DokumentService.Models.LogModel;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace DokumentService.Services.Logger
{
    public class LoggerService : ILoggerService
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _http;

        public LoggerService(IConfiguration configuration)
        {
            _configuration = configuration;
            _http = new HttpClient();
        }

        public async Task Log(LogLevel level, string method, string message, Exception exception = null)
        {
            var url = _configuration.GetValue<string>("Services:Logger");
            var log = new LogModel
            {
                Servis = "Dokument API",
                Level = level,
                Metoda = method,
                Poruka = message,
                Greska = exception
            };

            await _http.PostAsJsonAsync(url, log);
        }
    }
}