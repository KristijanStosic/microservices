using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KupacService.Model.Kupac.FizickoLice
{
    /// <summary>
    /// Dto za potvrdu kreiranja fizičkog lica
    /// </summary>
    public class FizickoLiceConfirmDto
    {
        /// <summary>
        /// Ime fizičkog lica
        /// </summary>
        public string Ime { get; set; }
        /// <summary>
        /// Prezime fizičkog lica
        /// </summary>
        public string Prezime { get; set; }
        /// <summary>
        /// JMBG fizičkog lica
        /// </summary>
        public string JMBG { get; set; }
        /// <summary>
        /// Broj telefona fizičkog lica
        /// </summary>
        public string BrojTelefona { get; set; }
        /// <summary>
        /// Email fizičkog lica
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// Broja računa fizičkog lica
        /// </summary>
        public string BrojRacuna { get; set; }
    }
}
