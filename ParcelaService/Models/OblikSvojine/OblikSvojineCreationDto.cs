using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ParcelaService.Models.OblikSvojine
{
    /// <summary>
    /// Model za kreiranje oblika svojine
    /// </summary>
    public class OblikSvojineCreationDto
    {
        /// <summary>
        /// Opis oblika svojine
        /// </summary>
        [Required(ErrorMessage ="Obavezno je uneti oblik svojine!")]
        public string OpisOblikaSvojine { get; set; }
    }
}
