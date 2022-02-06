using System;

namespace UgovorOZakupu.Models.JavnoNadmetanje
{
    /// <summary>
    ///     Model za etapu
    /// </summary>
    public class EtapaDto
    {
        /// <summary>
        ///     ID etape
        /// </summary>
        public Guid EtapaId { get; set; }

        /// <summary>
        ///     Datum odrzavanja etape u okviru javnog nadmetanja
        /// </summary>
        public DateTime Datum { get; set; }

        /// <summary>
        ///     Dan po redu etape u okviru javnog nadmetanja
        /// </summary>
        public int DanPoRedu { get; set; }

        /// <summary>
        ///     Vreme početka etape
        /// </summary>
        public string VremePocetka { get; set; }

        /// <summary>
        ///     Vreme kraja etape
        /// </summary>
        public string VremeKraja { get; set; }

        /// <summary>
        ///     Da li je etapa završena uspešno
        /// </summary>
        public bool ZavrsenaUspesno { get; set; }
    }
}