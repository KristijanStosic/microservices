using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrijavaService.Models.DokFizickoLice
{
    /// <summary>
    /// Model za dokument fizickog lica
    /// </summary>
    public class DokFizickaLicaDto
    {
        /// <summary>
        /// ID dokumenta fizickog lica
        /// </summary>
        public Guid DokFizickaLicaId{ get; set; }
        /// <summary>
        /// Naziv dokumenta fizickog lica
        /// </summary>
        public string NazivDokumenta { get; set; }
    }
}
