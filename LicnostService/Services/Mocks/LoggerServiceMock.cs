using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LicnostService.Models.OtherModels;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace LicnostService.Services.Mocks
{
    public class LoggerServiceMock : ILoggerService
    {
        public async Task<bool> Log(LogLevel level, string metoda, string poruka, Exception greska = null)
        {
            var log = new LoggerModel
            {
                Servis = "Licnost API",
                Level = level,
                Metoda = metoda,
                Poruka = poruka,
                Greska = greska
            };

            System.Diagnostics.Debug.WriteLine(JsonConvert.SerializeObject(log));

            return true;
        }

    }
}
