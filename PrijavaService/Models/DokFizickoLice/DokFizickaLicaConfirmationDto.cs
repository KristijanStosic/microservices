using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrijavaService.Models.DokFizickoLice
{
    /// <summary>
    /// Model za potvrdu kreiranja dokumenta fizickog lica
    /// </summary>
    public class DokFizickaLicaConfirmationDto
    {
        /// <summary>
        /// Naziv dokumenta fizickog lica
        /// </summary>
        public string NazivDokumenta { get; set; }
    }
}
