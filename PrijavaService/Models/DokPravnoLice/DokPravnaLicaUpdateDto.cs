using System;
using System.ComponentModel.DataAnnotations;

namespace PrijavaService.Models.DokPravnoLice
{
    public class DokPravnaLicaUpdateDto
    {
        /// <summary>
        /// ID dokumenta fizickog lica
        /// </summary>
        public Guid DokPravnaLicaId { get; set; }
        /// <summary>
        /// Naziv dokumenta fizickog lica
        /// </summary>
        public string NazivDokumenta { get; set; }
        /// <summary>
        /// Id prijave za koju se predaje dokument
        /// </summary>
        [Required(ErrorMessage = "Obavezno je uneti id prijave!")]
        public Guid PrijavaId { get; set; }
    }
}
