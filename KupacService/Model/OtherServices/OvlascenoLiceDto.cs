using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KupacService.Model.OtherServices
{
    /// <summary>
    /// Dto za ovlašćeno lice
    /// </summary>
    public class OvlascenoLiceDto
    {
        /// <summary>
        /// Naziv ovlašćenog lica
        /// </summary>
        public string OvlascenoLice { get; set; }
        /// <summary>
        /// Broj dokumenta
        /// </summary>
        public string BrojDokumenta { get; set; }
        /// <summary>
        /// Adresa ovlašćenog lica
        /// </summary>
        public string Stanovanje { get; set; }
    }
}
