using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ParcelaService.Models.ZasticenaZona
{
    /// <summary>
    /// Model za kreiranje zasticene zone parcele
    /// </summary>
    public class ZasticenaZonaCreationDto
    {
        /// <summary>
        /// Broj zasticene zone parcele
        /// </summary>
        [Required(ErrorMessage ="Obavezno je uneti broj zasticene zone!")]
        public string BrojZasticeneZone { get; set; }
    }
}
