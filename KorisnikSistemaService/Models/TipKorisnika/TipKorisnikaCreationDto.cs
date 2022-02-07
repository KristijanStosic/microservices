using System.ComponentModel.DataAnnotations;
namespace KorisnikSistemaService.Models.TipKorisnika
{
    /// <summary>
    /// Model za kreiranje tipa korisnika sistema
    /// </summary>
    public class TipKorisnikaCreationDto
    {
        /// <summary>
        /// Naziv tipa korisnika sistema
        /// </summary>
        [Required(ErrorMessage = "Obavezno je uneti naziv tipa korisnika sistema")]
        public string NazivTipaKorisnika { get; set; }
    }
}
