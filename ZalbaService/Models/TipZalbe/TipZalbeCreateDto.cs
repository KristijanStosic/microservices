using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ZalbaService.Models
{
    /// <summary>
    /// Creation Dto za tip žalbe
    /// </summary>
    public class TipZalbeCreateDto 
    {
        /// <summary>
        /// Naziv tipa žalbe
        /// </summary>
        [Required(ErrorMessage = "Obavezno je uneti naziv tipa zalbe")]
        public string NazivTipaZalbe { get; set; }
    }
}
