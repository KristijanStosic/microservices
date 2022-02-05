using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DokumentService.Entities
{
    [Table("TipDokumenta")]
    public class TipDokumenta
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public string NazivTipa { get; set; }
    }
}