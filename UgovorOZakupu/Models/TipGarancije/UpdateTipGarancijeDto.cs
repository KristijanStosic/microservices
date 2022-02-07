using System;
using System.ComponentModel.DataAnnotations;

namespace UgovorOZakupu.Models.TipGarancije
{
    /// <summary>
    ///     Model tipa garancije za izmenu
    /// </summary>
    public class UpdateTipGarancijeDto
    {
        /// <summary>
        ///     Id tipa garancije
        /// </summary>
        [Required(ErrorMessage = "Obavezno je id tipa garancije.")]
        public Guid Id { get; set; }

        /// <summary>
        ///     Naziv tipa garancije
        /// </summary>
        [Required(ErrorMessage = "Obavezno je uneti naziv tipa garancije.")]
        public string NazivTipa { get; set; }
    }
}