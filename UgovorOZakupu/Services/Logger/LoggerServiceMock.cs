using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using UgovorOZakupu.Models.LogModel;

namespace UgovorOZakupu.Services.Logger
{
    public class LoggerServiceMock : ILoggerService
    {
        public Task Log(LogLevel level, string method, string message, Exception exception = null)
        {
            var log = new LogModel
            {
                Servis = "Javno Nadmetanje API",
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