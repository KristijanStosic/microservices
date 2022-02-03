using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ParcelaService.Models.Obradivost
{
    /// <summary>
    /// Model za kreiranje obradivosti parcele
    /// </summary>
    public class ObradivostCreationDto
    {
        /// <summary>
        /// Opis obradivosti
        /// </summary>
        [Required(ErrorMessage ="Obavezno je uneti opis obradivosti!")]
        public string OpisObradivosti { get; set; }
    }
}
