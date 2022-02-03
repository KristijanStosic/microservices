using System;
using System.ComponentModel.DataAnnotations;

namespace PrijavaService.Models.DokFizickoLice
{
    public class DokFizickaLicaCreateDto
    {
        [Required(ErrorMessage = "Obavezno je uneti naziv dokumenta!")]
        public string NazivDokumenta { get; set; }

        [Required(ErrorMessage = "Obavezno je uneti id prijave!")]
        public Guid PrijavaId { get; set; }
    }
}
