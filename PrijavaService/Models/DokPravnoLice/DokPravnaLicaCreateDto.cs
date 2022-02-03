using System;
using System.ComponentModel.DataAnnotations;

namespace PrijavaService.Models.DokPravnoLice
{
    public class DokPravnaLicaCreateDto
    {
        [Required(ErrorMessage = "Obavezno je uneti naziv dokumenta!")]
        public string NazivDokumenta { get; set; }

        [Required(ErrorMessage = "Obavezno je uneti id prijave!")]
        public Guid PrijavaId { get; set; }
    }
}
