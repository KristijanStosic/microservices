using System;
using System.ComponentModel.DataAnnotations;

namespace JavnoNadmetanjeService.Models.Status
{
    /// <summary>
    /// Model za izmenu statusa javnog nadmetanja
    /// </summary>
    public class StatusUpdateDto
    {
        /// <summary>
        /// Id statusa javnog nadmetanja
        /// </summary>
        public Guid StatusId { get; set; }
        /// <summary>
        /// Naziv statusa javnog nadmetanjaa
        /// </summary>
        [Required(ErrorMessage = "Obavezno je uneti naziv statusa javnog nadmetanja")]
        public string NazivStatusa { get; set; }
    }
}
