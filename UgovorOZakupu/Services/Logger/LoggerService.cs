using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using UgovorOZakupu.Models.LogModel;

namespace UgovorOZakupu.Services.Logger
{
    public class LoggerService : ILoggerService
    {
        private IConfiguration _configuration;
        private HttpClient _http;

        public LoggerService(IConfiguration configuration)
        {
            _configuration = configuration;
            _http = new HttpClient();
        }

        public async Task Log(LogLevel level, string method, string message, Exception exception = null)
        {
            var log = new LogModel
            {
                Servis = "Ugovor o zakupu API",
                Level = level,
                Metoda = method,
                Poruka = message,
                Greska = exception
            };

            var uri = _configuration.GetValue<string>("Services:Logger");
            await _http.PostAsJsonAsync(uri, log);
        }
    }
}