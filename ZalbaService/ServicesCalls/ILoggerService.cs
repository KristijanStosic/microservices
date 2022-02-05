using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZalbaService.ServicesCalls
{
    /// <summary>
    /// Interfejs loger servisa
    /// </summary>
    public interface ILoggerService
    {
        /// <summary>
        /// Slanje post zahteva za upis novog log zapisa
        /// </summary>
        /// <param name="level">INFO/DEBUG/WARN/ERROR</param>
        /// <param name="metoda">Naziv metode</param>
        /// <param name="poruka">Tekst poruke</param>
        /// <param name="greska">Exception</param>
        /// <returns></returns>
        Task<bool> Log(LogLevel level, string metoda, string poruka, Exception greska = null);
    }
}
