using System;
using System.ComponentModel.DataAnnotations;

namespace PrijavaService.Entities
{
    public class DokFizickaLica
    {
        [Key]
        public Guid DokFizickaLicaId { get; set; } = Guid.NewGuid();
        [Required]
        public string NazivDokumenta { get; set; }


        public Guid PrijavaId { get; set; }
        public Prijava Prijava { get; set; }


    }
}
