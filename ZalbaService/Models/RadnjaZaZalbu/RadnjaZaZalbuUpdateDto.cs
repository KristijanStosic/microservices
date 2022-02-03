using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ZalbaService.Models
{
    /// <summary>
    /// Update Dto za radnju žalbe
    /// </summary>
    public class RadnjaZaZalbuUpdateDto
    {
        /// <summary>
        /// Naziv radnje žalbe
        /// </summary>
        [Required(ErrorMessage = "Obavezno je uneti naziv radnje za zalbu")]
        public string NazivRadnjeZaZalbu { get; set; }
    }
}
