using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KupacService.Model.KontaktOsoba
{
    public class KontaktOsobaUpdateDto
    {
        /// <summary>
        /// Id kontakt osobe
        /// </summary>
        [Required]
        public Guid KontaktOsobaId { get; set; } = Guid.NewGuid();
        /// <summary>
        /// Ime kontakt osobe
        /// </summary>
        public string Ime { get; set; }
        /// <summary>
        /// Prezime kontakt osobe
        /// </summary>
        public string Prezime { get; set; }
        /// <summary>
        /// Broj telefona kontakt osobe
        /// </summary>
        public string Telefon { get; set; }
        /// <summary>
        /// Funkcija kontakt osobe
        /// </summary>
        public string Funkcija { get; set; }
    }
}
