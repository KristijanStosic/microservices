using ParcelaService.Models.OtherServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParcelaService.Models.DeoParcele
{
    public class DeoParceleDto
    {
        /// <summary>
        /// Id dela parcele
        /// </summary>
        public Guid DeoParceleId { get; set; }
        /// <summary>
        /// Redni broj dela parcele
        /// </summary>
        public string RedniBrojDela { get; set; }
        /// <summary>
        /// Povrsina dela parcele
        /// </summary>
        public string PovrsinaDela { get; set; }
        /// <summary>
        /// Broj parcele
        /// </summary>
        public string BrojParcele { get; set; }
        /// <summary>
        /// Kultura
        /// </summary>
        public string Kultura { get; set; }
        /// <summary>
        /// Klasa
        /// </summary>
        public string Klasa { get; set; }
        /// <summary>
        /// Obradivost
        /// </summary>
        public string Obradivost { get; set; }
        /// <summary>
        /// Katastarska opština
        /// </summary>
        public string KatastarskaOpstina { get; set; }
        /// <summary>
        /// Oblik svojine
        /// </summary>
        public string OblikSvojine { get; set; }
        /// <summary>
        /// Zastićena zona
        /// </summary>
        public string ZasticenaZona { get; set; }
        /// <summary>
        /// Odvodnjavanje
        /// </summary>
        public string Odvodnjavanje { get; set; }
        /// <summary>
        /// Kupac
        /// </summary>
        public KupacDto Kupac { get; set; }
    }
}
