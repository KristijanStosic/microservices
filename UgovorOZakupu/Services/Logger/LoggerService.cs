using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using UgovorOZakupu.Models.LogModel;

namespace UgovorOZakupu.Services.Logger
{
    public class LoggerServiceBase : ServiceBase, ILoggerService
    {
        public LoggerServiceBase(IConfiguration configuration) : base(configuration.GetValue<string>("Services:Logger"))
        {
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

            await SendPostRequest(log);
        }
    }
}