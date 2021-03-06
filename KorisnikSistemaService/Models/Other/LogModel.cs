using Microsoft.Extensions.Logging;
using System;

namespace KorisnikSistemaService.Models.Other
{
    public class LogModel
    {
        public LogLevel Level { get; set; }
        public string Servis { get; set; }
        public string Metoda { get; set; }
        public string Poruka { get; set; }
        public Exception Greska { get; set; }
    }
}
