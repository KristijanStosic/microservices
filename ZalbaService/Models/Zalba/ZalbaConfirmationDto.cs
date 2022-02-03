using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZalbaService.Models.Zalba
{
    /// <summary>
    /// Confirmation Dto za žalbu
    /// </summary>
    public class ZalbaConfirmationDto
    {
        /// <summary>
        /// Broj nadmetanja
        /// </summary>
        public string BrojNadmetanja { get; set; }
        /// <summary>
        /// Broj rešenja
        /// </summary>
        public string BrojResenja { get; set; }
        /// <summary>
        /// Datum podnošenja
        /// </summary>
        public DateTime DatumPodnosenja { get; set; }
        /// <summary>
        /// Datum rešenja
        /// </summary>
        public DateTime DatumResenja { get; set; }
    }
}
