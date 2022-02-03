using System;
using System.ComponentModel.DataAnnotations;

namespace PrijavaService.Models.DokPravnoLice
{
    public class DokPravnaLicaUpdateDto
    {
        public Guid DokPravnaLicaId { get; set; }
        public string NazivDokumenta { get; set; }
        [Required(ErrorMessage = "Obavezno je uneti id prijave!")]
        public Guid PrijavaId { get; set; }
    }
}
