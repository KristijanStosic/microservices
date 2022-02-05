using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LicnostService.Models.Licnost
{
    /// <summary>
    /// Model za izmenu ličnosti
    /// </summary>
    public class LicnostUpdateDTO
    {
        /// <summary>
        /// ID Ličnosti
        /// </summary>
        public Guid LicnostId { get; set; }
        [Required(ErrorMessage = "Obavezno je uneti ime licnosti.")]

        /// <summary>
        /// Ime ličnosti
        /// </summary>
        public string Ime { get; set; }
        [Required(ErrorMessage = "Obavezno je uneti prezime licnosti.")]
        
        /// <summary>
        /// Prezime ličnosti
        /// </summary>
        public string Prezime { get; set; }
        [Required(ErrorMessage = "Obavezno je uneti funkciju licnosti.")]
        
        /// <summary>
        /// Funkcija ličnosti
        /// </summary>
        public string Funkcija { get; set; }
    }
}
