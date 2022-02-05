using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ParcelaService.Models.Kultura
{
    /// <summary>
    /// Model za kreiranje kulture parcele
    /// </summary>
    public class KulturaCreationDto
    {
        /// <summary>
        /// Naziv kulture
        /// </summary>
        [Required(ErrorMessage = "Obavezno je uneti naziv kulture!")]
        public string NazivKulture { get; set; }
    }
}
