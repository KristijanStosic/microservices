using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

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
