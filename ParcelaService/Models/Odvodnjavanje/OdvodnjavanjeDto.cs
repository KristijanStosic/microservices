using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParcelaService.Models.Odvodnjavanje
{
    /// <summary>
    /// Model za odvodnjavanje parcele
    /// </summary>
    public class OdvodnjavanjeDto
    {
        /// <summary>
        /// Id odvodnjavanja parcele
        /// </summary>
        public Guid OdvodnjavanjeId { get; set; }
        /// <summary>
        /// Opis odvodnjavanja parcele
        /// </summary>
        public string OpisOdvodnjavanja { get; set; }
    }
}
