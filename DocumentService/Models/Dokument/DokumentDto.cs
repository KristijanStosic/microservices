using System;

namespace DocumentService.Models.Dokument
{
    public class DokumentDto
    {
        public string ZavodniBroj { get; set; }

        public DateTime Datum { get; set; } = DateTime.Now;

        public DateTime DatumDonosenjaDokumenta { get; set; } = DateTime.Now;

        public string TipDokumenta { get; set; }
    }
}