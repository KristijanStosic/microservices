using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LicnostService.Models.OtherModels
{
    /// <summary>
    /// Model za etapu
    /// </summary>
    public class DokumentDTO
    {
        /// <summary>
        /// Zavodni broj dokumenta
        /// </summary>
        public string ZavodniBroj { get; set; }

        /// <summary>
        /// Datum donošenja dokumenta
        /// </summary>
        public DateTime DatumDonosenjaDokumenta { get; set; }

        /// <summary>
        /// Tip dokumenta
        /// </summary>
        public string TipDokumenta { get; set; }

        

    }
}
