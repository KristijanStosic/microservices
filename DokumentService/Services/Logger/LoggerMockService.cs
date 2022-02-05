using System;
using System.Threading.Tasks;
using DokumentService.Models.LogModel;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace DokumentService.Services.Logger
{
    public class LoggerMockService : ILoggerService
    {
        public Task Log(LogLevel level, string method, string message, Exception exception = null)
        {
            var log = new LogModel
            {
                Servis = "Dokument API",
                Level = level,
                Metoda = method,
                Poruka = message,
                Greska = exception
            };
            
            System.Diagnostics.Debug.WriteLine(JsonConvert.SerializeObject(log));
            
            return Task.CompletedTask;
        }
    }
}