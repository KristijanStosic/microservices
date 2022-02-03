using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZalbaService.Entities.Confirmations
{
    /// <summary>
    /// Predstavlja potvrdu kreiranja zalbe 
    /// </summary>
    public class ZalbaConfirmation
    {
        /// <summary>
        /// ID žalbe
        /// </summary>
        public Guid ZalbaId { get; set; }
        /// <summary>
        /// Broj nadmetanja
        /// </summary>
        public string BrojNadmetanja { get; set; }
        /// <summary>
        /// Broj resenja
        /// </summary>
        public string BrojResenja { get; set; }
        /// <summary>
        /// Datum podnosenja
        /// </summary>
        public DateTime DatumPodnosenja { get; set; }
        /// <summary>
        /// Datum resenja
        /// </summary>
        public DateTime DatumResenja { get; set; }
    }
}
