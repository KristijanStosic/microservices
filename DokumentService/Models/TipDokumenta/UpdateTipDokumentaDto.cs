using System;
using System.ComponentModel.DataAnnotations;

namespace DokumentService.Models.TipDokumenta
{
    /// <summary>
    ///     Model za izmenu tipa dokumenta
    /// </summary>
    public class UpdateTipDokumentaDto
    {
        /// <summary>
        ///     Id tipa dokumenta
        /// </summary>
        [Required(ErrorMessage = "Obavezno je uneti id tipa dokumenta.")]
        public Guid Id { get; set; }

        /// <summary>
        ///     Naziv tipa dokumenta
        /// </summary>
        [Required(ErrorMessage = "Obavezno je naziv tipa dokumenta.")]
        public string NazivTipa { get; set; }
    }
}