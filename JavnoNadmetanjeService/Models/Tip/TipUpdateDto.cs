using System;
using System.ComponentModel.DataAnnotations;

namespace JavnoNadmetanjeService.Models.Tip
{
    /// <summary>
    /// Model za izmenu tipa javnog nadmetanja
    /// </summary>
    public class TipUpdateDto
    {
        /// <summary>
        /// Id tipa javnog nadmetanja
        /// </summary>
        public Guid TipId { get; set; }
        /// <summary>
        /// Naziv tipa javnog nadmetanja
        /// </summary>
        [Required(ErrorMessage = "Obavezno je uneti naziv tipa javnog nadmetanja")]
        public string NazivTipa { get; set; }
    }
}
