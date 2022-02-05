using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ZalbaService.Models
{
    /// <summary>
    /// Update Dto za status žalbe
    /// </summary>
    public class StatusZalbeUpdateDto
    {
        /// <summary>
        /// Naziv statusa žalbe
        /// </summary>
        [Required(ErrorMessage = "Obavezno je uneti naziv statusa zalbe")]
        public string NazivStatusaZalbe { get; set; }
    }
}
