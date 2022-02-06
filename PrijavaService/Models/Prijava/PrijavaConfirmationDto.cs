using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrijavaService.Models.Prijava
{
    public class PrijavaConfirmationDto
    {
        /// <summary>
        /// Registarski broj prijave
        /// </summary>
        public string BrojPrijave { get; set; }
        /// <summary>
        /// Datum kada je prijava podnijeta
        /// </summary>
        public DateTime DatumPrijave { get; set; }
        /// <summary>
        /// Mjesto u kojem je podnijeta prijava
        /// </summary>
        public string MestoPrijave { get; set; }
    }
}
