using System;

namespace DocumentService.Models.Dokument
{
    public class UpdateDokumentDto
    {
        public Guid Id { get; set; }
        
        public string ZavodniBroj { get; set; }

        public DateTime Datum { get; set; }

        public DateTime DatumDonosenjaDokumenta { get; set; }

        public Guid TipDokumentaId { get; set; }
    }
}