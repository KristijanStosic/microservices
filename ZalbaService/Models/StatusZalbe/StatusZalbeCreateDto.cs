using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ZalbaService.Models
{
    /// <summary>
    /// Creation Dto za status žalbe
    /// </summary>
    public class StatusZalbeCreateDto
    {
        /// <summary>
        /// Naziv statusa žalbe
        /// </summary>
        [Required(ErrorMessage = "Obavezno je uneti naziv statusa zalbe")]
        public string NazivStatusaZalbe { get; set; }
    }
}
