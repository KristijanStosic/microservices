using System;

namespace DokumentService.Models.Dokument
{
    /// <summary>
    /// Model za kreiranje dokumenta
    /// </summary>
    public class CreateDokumentDto
    {
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
        /// Tip dokumenta
        /// </summary>
        public Guid TipDokumentaId { get; set; }
    }
}