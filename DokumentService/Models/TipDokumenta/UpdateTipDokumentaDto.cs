using System;

namespace DokumentService.Models.TipDokumenta
{
    /// <summary>
    /// Model za izmenu tipa dokumenta
    /// </summary>
    public class UpdateTipDokumentaDto
    { 
        /// <summary>
        /// Id tipa dokumenta
        /// </summary>
        public Guid Id { get; set; }
        
        /// <summary>
        /// Naziv tipa dokumenta
        /// </summary>
        public string NazivTipa { get; set; }
    }
}