using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LicitacijaService.Models.OtherServices
{
    /// <summary>
    /// Model za etapu
    /// </summary>
    public class EtapaDto
    {
        /// <summary>
        /// ID etape
        /// </summary>
        public Guid EtapaId { get; set; }
        /// <summary>
        /// Datum odrzavanja etape u okviru javnog nadmetanja
        /// </summary>
        public DateTime Datum { get; set; }
        /// <summary>
        /// Dan po redu etape u okviru javnog nadmetanja
        /// </summary>
        public int DanPoRedu { get; set; }
        /// <summary>
        /// Vreme pocetka etape
        /// </summary>
        public string VremePocetka { get; set; }
        /// <summary>
        /// Vreme kraja etape
        /// </summary>
        public string VremeKraja { get; set; }
        /// <summary>
        /// Da li je etapa zavrsena uspesno
        /// </summary>
        public bool ZavrsenaUspesno { get; set; }


    }
}
