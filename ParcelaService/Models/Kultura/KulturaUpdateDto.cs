using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ParcelaService.Models.Kultura
{
    /// <summary>
    /// Model za izmenu kulture parcele
    /// </summary>
    public class KulturaUpdateDto
    {
        /// <summary>
        /// Id kulture parcele
        /// </summary>
        public Guid KulturaId { get; set; }
        /// <summary>
        /// Naziv kulture parcele
        /// </summary>
        [Required(ErrorMessage ="Obavezno je uneti naziv kulture!")]
        public string NazivKulture { get; set; }
    }
}
