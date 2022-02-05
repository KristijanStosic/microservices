using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LicnostService.Models.Licnost
{
    /// <summary>
    /// Model za kreiranje ličnosti
    /// </summary>
    public class LicnostCreateDto
    {
        /// <summary>
        /// Ime ličnosti
        /// </summary>
        [Required(ErrorMessage="Obavezno je uneti ime licnosti.")]
        public string Ime { get; set; }

        /// <summary>
        /// Prezime ličnosti
        /// </summary>
        [Required(ErrorMessage = "Obavezno je uneti prezime licnosti.")]
        public string Prezime { get; set; }

        /// <summary>
        /// Funkcija ličnosti
        /// </summary>
        [Required(ErrorMessage = "Obavezno je uneti funkciju licnosti.")]
        public string Funkcija { get; set; }
    }
}
