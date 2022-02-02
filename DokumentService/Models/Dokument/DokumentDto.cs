using System;

namespace DokumentService.Models.Dokument
{
    /// <summary>
    /// Model dokumenta
    /// </summary>
    public class DokumentDto
    {
        /// <summary>
        /// Zavodni broj dokumenta
        /// </summary>
        public string ZavodniBroj { get; set; }

        /// <summary>
        /// Datum kreiranja dokumenta
        /// </summary>
        public DateTime Datum { get; set; } = DateTime.Now;

        /// <summary>
        /// Datum donošenja dokumenta
        /// </summary>
        public DateTime DatumDonosenjaDokumenta { get; set; } = DateTime.Now;

        /// <summary>
        /// Tip dokumenta
        /// </summary>
        public string TipDokumenta { get; set; }
    }
}