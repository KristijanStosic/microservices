using System;
using System.ComponentModel.DataAnnotations;

namespace PrijavaService.Models.DokFizickoLice
{
    public class DokFizickaLicaUpdateDto
    {
        public Guid DokFizickaLicaId { get; set; }
        public string NazivDokumenta { get; set; }
        [Required(ErrorMessage = "Obavezno je uneti id prijave!")]
        public Guid PrijavaId { get; set; }
    }
}
