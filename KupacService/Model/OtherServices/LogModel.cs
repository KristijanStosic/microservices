using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KupacService.Model.OtherServices
{
    public class LogModel
    {
        /// <summary>
        /// Log level - info, warn, error, debug
        /// </summary>
        public LogLevel Level { get; set; }
        /// <summary>
        /// Naziv servisa
        /// </summary>
        public string Servis { get; set; }
        /// <summary>
        /// Naziv metode
        /// </summary>
        public string Metoda { get; set; }
        /// <summary>
        /// Tekst poruke
        /// </summary>
        public string Poruka { get; set; }
        /// <summary>
        /// Greska
        /// </summary>
        public Exception Greska { get; set; }
    }
}
