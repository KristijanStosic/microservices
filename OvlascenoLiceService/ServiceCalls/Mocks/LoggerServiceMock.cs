using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using OvlascenoLiceService.Models.OtherServices;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace OvlascenoLiceService.ServiceCalls.Mocks
{
    /// <summary>
    /// Logger service mock
    /// </summary>
    public class LoggerServiceMock : ILoggerService
    {
        /// <summary>
        /// Slanje post zahteva za upis novog log zapisa - mock
        /// </summary>
        /// <param name="level">INFO/DEBUG/WARN/ERROR</param>
        /// <param name="metoda">Naziv metode</param>
        /// <param name="poruka">Tekst poruke</param>
        /// <param name="greska">Exception</param>
        /// <returns></returns>
        public async Task<bool> Log(LogLevel level, string metoda, string poruka, Exception greska = null)
        {
            var log = new LogModel
            {
                Servis = "Ovlasceno lice API",
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
