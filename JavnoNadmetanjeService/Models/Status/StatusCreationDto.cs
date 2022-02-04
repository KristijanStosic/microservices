using System.ComponentModel.DataAnnotations;

namespace JavnoNadmetanjeService.Models.Status
{
    /// <summary>
    /// Model za kreiranje statusa javnog nadmetanja
    /// </summary>
    public class StatusCreationDto
    {
        /// <summary>
        /// Naziv statusa javnog nadmetanjaa
        /// </summary>
        [Required(ErrorMessage = "Obavezno je uneti naziv statusa javnog nadmetanja")]
        public string NazivStatusa { get; set; }
    }
}
