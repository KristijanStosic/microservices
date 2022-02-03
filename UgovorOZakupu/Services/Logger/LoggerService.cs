using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using UgovorOZakupu.Models.LogModel;

namespace UgovorOZakupu.Services.Logger
{
    public class LoggerService : Service<LogModel>, ILoggerService
    {
        private IConfiguration _configuration;
        
        public LoggerService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task Log(LogLevel level, string method, string message, Exception exception = null)
        {
            var log = new LogModel
            {
                Servis = "Dokument API",
                Level = level,
                Metoda = method,
                Poruka = message,
                Greska = exception
            };

            await SendPostRequest(_configuration.GetValue<string>("Services:Logger"), log);
        }
    }
}