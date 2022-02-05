using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZalbaService.Models.Services
{
    /// <summary>
    /// Dto Kupac - Entitet iz drugog agregata
    /// </summary>
    public class KupacDto
    {
        /// <summary>
        /// Naziv kupca
        /// </summary>
        public string Kupac { get; set; }
        /// <summary>
        /// Email
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// Broj racuna
        /// </summary>
        public string BrojRacuna { get; set; }
        /// <summary>
        /// Broj telefona 1
        /// </summary>
        public string BrojTelefona1 { get; set; }

    }
}
