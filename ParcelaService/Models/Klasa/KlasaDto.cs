using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParcelaService.Models.Klasa
{
    /// <summary>
    /// Model za klasu parcele
    /// </summary>
    public class KlasaDto
    {
        /// <summary>
        /// Id klase parcele
        /// </summary>
        public Guid KlasaId { get; set; }

        /// <summary>
        /// Naziv klase 
        /// </summary>
        public string KlasaNaziv { get; set; }
    }
}
