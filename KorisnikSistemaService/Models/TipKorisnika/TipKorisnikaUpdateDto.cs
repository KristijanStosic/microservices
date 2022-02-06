using System;
using System.ComponentModel.DataAnnotations;


namespace KorisnikSistemaService.Models.TipKorisnika
{
    /// <summary>
    /// Model za izmenu tipa korisnika sistema
    /// </summary>
    public class TipKorisnikaUpdateDto
    {
        /// <summary>
        /// Id tipa korisnika sistema
        /// </summary>
        public Guid TipKorisnikaId { get; set; }
        /// <summary>
        /// Naziv tipa korisnika sistema
        /// </summary>
        [Required(ErrorMessage = "Obavezno je uneti naziv tipa korisnika sistema")]
        public string NazivTipaKorisnika { get; set; }
    }
}
