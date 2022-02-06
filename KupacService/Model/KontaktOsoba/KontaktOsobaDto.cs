using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KupacService.Model.KontaktOsoba
{
    /// <summary>
    /// Dto za kontakt osobu
    /// </summary>
    public class KontaktOsobaDto
    {
        /// <summary>
        /// id kontakt osobe
        /// </summary>
        [Required]
        public Guid KontaktOsobaId { get; set; }
        /// <summary>
        /// ime kontakt osobe
        /// </summary>
        public string Ime { get; set; }
        /// <summary>
        /// Prezime kontak osobe
        /// </summary>
        public string Prezime { get; set; }
        /// <summary>
        /// Broj telefona kontakt osobe
        /// </summary>
        public string Telefon { get; set; }
        /// <summary>
        /// Fukncija kontakt osobe
        /// </summary>
        public string Funkcija { get; set; }
    }
}
