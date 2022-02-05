using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DokumentService.Entities
{
    [Table("TipDokumenta")]
    public class TipDokumenta
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string NazivTipa { get; set; }
    }
}