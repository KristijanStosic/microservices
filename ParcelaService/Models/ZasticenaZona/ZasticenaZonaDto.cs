using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParcelaService.Models.ZasticenaZona
{
    /// <summary>
    /// Model za zasticenu zonu parcele
    /// </summary>
    public class ZasticenaZonaDto
    {
        /// <summary>
        /// Id broja zasticene zone parcele
        /// </summary>
        public Guid ZasticenaZonaId { get; set; }
        /// <summary>
        /// Broj zasticene zone parcele
        /// </summary>
        public string BrojZasticeneZone { get; set; }
    }
}
