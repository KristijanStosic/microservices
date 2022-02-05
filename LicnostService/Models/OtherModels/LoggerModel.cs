using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace LicnostService.Models.OtherModels
{
    /// <summary>
    /// Model za logger
    /// </summary>
    public class LoggerModel
    {
        /// <summary>
        /// Nivo HTTP status kod-a
        /// </summary>
        public LogLevel Level { get; set; }

        /// <summary>
        /// Servis sa kojeg se loguje poruka
        /// </summary>
        public string Servis { get; set; }

        /// <summary>
        /// Metoda koja se loguje
        /// </summary>
        public string Metoda { get; set; }

        /// <summary>
        /// Dodatni opis poruke prilikom logovanja
        /// </summary>
        public string Poruka { get; set; }

        /// <summary>
        /// Greška prilikom logovanja
        /// </summary>
        public Exception Greska { get; set; }
    }
}
