using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KupacService.Model.Kupac.PravnoLice
{   /// <summary>
/// Dto za potvrdu kreiranja pravnog lica
/// </summary>
    public class PravnoLiceConfirmDto
    {
        /// <summary>
        /// Naziv pravnog lica
        /// </summary>
        public string Naziv { get; set; }
        /// <summary>
        /// Matični broj pravnog lica
        /// </summary>
        public string MaticniBroj { get; set; }
        /// <summary>
        /// Broj telefona pravnog lica
        /// </summary>
        public string BrojTelefona { get; set; }
        /// <summary>
        /// Email pravnog lica
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// Broj računa pravnog lica
        /// </summary>
        public string BrojRacuna { get; set; }
    }
}
