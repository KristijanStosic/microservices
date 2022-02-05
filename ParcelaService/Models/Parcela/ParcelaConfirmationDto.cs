using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParcelaService.Models.Parcela
{
    public class ParcelaConfirmationDto
    {
        /// <summary>
        /// Broj parcele
        /// </summary>
        public string BrojParcele { get; set; }
        /// <summary>
        /// Površina parcele
        /// </summary>
        public string PovrsinaParcele { get; set; }
        /// <summary>
        /// Broj liste nepokretnosti
        /// </summary>
        public string BrojListeNepokretnosti { get; set; }
    }
}
