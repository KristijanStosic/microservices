using System;
using Microsoft.Extensions.Logging;

namespace DokumentService.Models.LogModel
{
    /// <summary>
    ///     Model za logger
    /// </summary>
    public class LogModel
    {
        /// <summary>
        ///     Nivo Log levela - Info,Warning,Error,Debug
        /// </summary>
        public LogLevel Level { get; set; }

        /// <summary>
        ///     Servis sa kojeg se loguje poruka
        /// </summary>
        public string Servis { get; set; }

        /// <summary>
        ///     Metoda koja se loguje
        /// </summary>
        public string Metoda { get; set; }

        /// <summary>
        ///     Dodatni opis poruke prilikom logovanja
        /// </summary>
        public string Poruka { get; set; }

        /// <summary>
        ///     Greška prilikom logovanja
        /// </summary>
        public Exception Greska { get; set; }
    }
}