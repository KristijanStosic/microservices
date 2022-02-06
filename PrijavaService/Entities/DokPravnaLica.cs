using System;
using System.ComponentModel.DataAnnotations;

namespace PrijavaService.Entities
{
    public class DokPravnaLica
    {
        [Key]
        public Guid DokPravnaLicaId { get; set; } = Guid.NewGuid();

        [Required]
        public string NazivDokumenta { get; set; }

        public Guid PrijavaId { get; set; }
        public Prijava Prijava { get; set; }
    }
}
