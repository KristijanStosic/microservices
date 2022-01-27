using System;

namespace OvlascenoLiceService.Entities.Confirmations
{
    /// <summary>
    /// Predstavlja potvrdu kreiranja ovlascenog lica
    /// </summary>
    public class OvlascenoLiceConfirmation
    {
        /// <summary>
        /// ID ovlascenog lica
        /// </summary>
        public Guid OvlascenoLiceId { get; set; }
        /// <summary>
        /// Ime ovlascenog lica
        /// </summary>
        public string Ime { get; set; }
        /// <summary>
        /// Prezime ovlascenog lica
        /// </summary>
        public string Prezime { get; set; }
    }
}
