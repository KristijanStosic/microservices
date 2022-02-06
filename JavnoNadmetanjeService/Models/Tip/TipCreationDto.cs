using System.ComponentModel.DataAnnotations;

namespace JavnoNadmetanjeService.Models.Tip
{
    /// <summary>
    /// Model za kreiranje tipa javnog nadmetanja
    /// </summary>
    public class TipCreationDto 
    {
        /// <summary>
        /// Naziv tipa javnog nadmetanja
        /// </summary>
        [Required(ErrorMessage = "Obavezno je uneti naziv tipa javnog nadmetanja")]
        public string NazivTipa { get; set; }
    }
}
