using System;

namespace DokumentService.Models.Dokument
{
    /// <summary>
    /// Model za izmenu dokumenta
    /// </summary>
    public class UpdateDokumentDto
    {
        /// <summary>
        /// Id dokumenta
        /// </summary>
        public Guid Id { get; set; }
        
        /// <summary>
        /// Zavodni broj dokumenta
        /// </summary>
        public string ZavodniBroj { get; set; }

        /// <summary>
        /// Datum kreiranja dokumenta
        /// </summary>
        public DateTime Datum { get; set; }

        /// <summary>
        /// Datum donošenja dokumenta
        /// </summary>
        public DateTime DatumDonosenjaDokumenta { get; set; }

        /// <summary>
        /// Id tipa dokumenta
        /// </summary>
        public Guid TipDokumentaId { get; set; }
    }
}