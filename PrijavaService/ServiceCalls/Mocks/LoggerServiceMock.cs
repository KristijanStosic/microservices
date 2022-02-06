using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PrijavaService.Models.Other;
using System;
using System.Threading.Tasks;

namespace PrijavaService.ServiceCalls.Mocks
{
    public class LoggerServiceMock : ILoggerService
    {
        public async Task<bool> Log(LogLevel level, string metoda, string poruka, Exception greska = null)
        {
            var log = new LogModel
            {
                Servis = "Prijava API",
                Level = level,
                Metoda = metoda,
                Poruka = poruka,
                Greska = greska
            };

            System.Diagnostics.Debug.WriteLine(JsonConvert.SerializeObject(log));

            return await Task.FromResult(true);
        }
    }
}
