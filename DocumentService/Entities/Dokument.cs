using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DocumentService.Entities
{
    [Table("Dokument")]
    public class Dokument
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string ZavodniBroj { get; set; }

        public DateTime Datum { get; set; } = DateTime.Now;

        public DateTime DatumDonosenjaDokumenta { get; set; } = DateTime.Now;

        public Guid TipDokumentaId { get; set; }
        public TipDokumenta TipDokumenta { get; set; }
    }
}