using System;
using System.ComponentModel.DataAnnotations;

namespace PrijavaService.Models.DokFizickoLice
{
    /// <summary>
    /// Model za kreiranje dokumenta fizickog lica
    /// </summary>
    public class DokFizickaLicaCreateDto
    {
        /// <summary>
        /// Naziv dokumenta fizickog lica
        /// </summary>
        [Required(ErrorMessage = "Obavezno je uneti naziv dokumenta!")]
        public string NazivDokumenta { get; set; }
        /// <summary>
        /// Id prijave za koju se predaje dokument
        /// </summary>
        [Required(ErrorMessage = "Obavezno je uneti id prijave!")]
        public Guid PrijavaId { get; set; }
    }
}
