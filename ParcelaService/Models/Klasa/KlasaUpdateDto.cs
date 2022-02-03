using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ParcelaService.Models.Klasa
{
    /// <summary>
    /// Model za izmenu klase parcele
    /// </summary>
    public class KlasaUpdateDto
    {
        /// <summary>
        /// Id klase parcele
        /// </summary>
        public Guid KlasaId { get; set; }
        /// <summary>
        /// Naziv klase
        /// </summary>
        [Required(ErrorMessage ="Obavezno je uneti naziv klase!")]
        public string KlasaNaziv { get; set; }
    }
}
