using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ParcelaService.Models.Klasa
{
    /// <summary>
    /// Model za kreiranje klase parcele
    /// </summary>
    public class KlasaCreationDto
    {

        /// <summary>
        /// Naziv klase parcele
        /// </summary>
        [Required(ErrorMessage ="Obavezno je uneti naziv klase!")]
        public string KlasaNaziv { get; set; }
    }
}
