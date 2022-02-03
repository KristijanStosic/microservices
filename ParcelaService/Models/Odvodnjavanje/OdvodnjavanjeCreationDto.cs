using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ParcelaService.Models.Odvodnjavanje
{
    /// <summary>
    /// Model za kreiranje odvodnjavanja parcele
    /// </summary>
    public class OdvodnjavanjeCreationDto
    {
        /// <summary>
        /// Opis odvodnjavanja parcele
        /// </summary>
        [Required(ErrorMessage ="Obavezno je uneti opis odvodnjavanja!")]
        public string OpisOdvodnjavanja { get; set; }
    }
}
