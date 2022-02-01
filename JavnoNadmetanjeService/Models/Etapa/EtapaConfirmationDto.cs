using System;

namespace JavnoNadmetanjeService.Models.Etapa
{
    /// <summary>
    /// Model za potvrdu kreiranja etape
    /// </summary>
    public class EtapaConfirmationDto
    {
        /// <summary>
        /// Datum odrzavanja etape
        /// </summary>
        public DateTime Datum { get; set; }
        /// <summary>
        /// Dan po redu u okviru javnog nadmetanja
        /// </summary>
        public int DanPoRedu { get; set; }
        /// <summary>
        /// Vreme pocetka etapee
        /// </summary>
        public string VremePocetka { get; set; }
    }
}
