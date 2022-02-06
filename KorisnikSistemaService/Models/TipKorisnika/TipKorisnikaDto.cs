using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KorisnikSistemaService.Models.TipKorisnika
{
    /// <summary>
    /// Model za tip korisnika sistema
    /// </summary>
    public class TipKorisnikaDto
    {
        /// <summary>
        /// Id tipa korisnika
        /// </summary>
        public Guid TipKorisnikaId { get; set; }
        /// <summary>
        /// Naziv tipa korisnika
        /// </summary>
        public string NazivTipaKorisnika { get; set; }
    }
}
