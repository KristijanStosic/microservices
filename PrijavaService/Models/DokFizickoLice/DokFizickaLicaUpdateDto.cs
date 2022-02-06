using System;
using System.ComponentModel.DataAnnotations;

namespace PrijavaService.Models.DokFizickoLice
{
    /// <summary>
    /// Model za izmenu dokumenta fifickog lica
    /// </summary>
    public class DokFizickaLicaUpdateDto
    {
        /// <summary>
        /// ID dokumenta fizickog lica
        /// </summary>
        public Guid DokFizickaLicaId { get; set; }
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
