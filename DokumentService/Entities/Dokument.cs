using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DokumentService.Entities
{
    [Table("Dokument")]
    public class Dokument
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public string ZavodniBroj { get; set; }

        public DateTime Datum { get; set; } = DateTime.Now;

        public DateTime DatumDonosenjaDokumenta { get; set; }

        [Required]
        public Guid TipDokumentaId { get; set; }
        public TipDokumenta TipDokumenta { get; set; }
    }
}