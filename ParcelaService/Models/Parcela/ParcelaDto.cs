using ParcelaService.Models.DeoParcele;
using System;
using System.Collections.Generic;

namespace ParcelaService.Models.Parcela
{
    public class ParcelaDto
    {
        /// <summary>
        /// Id parcele
        /// </summary>
        public Guid ParcelaId { get; set; }
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
        /// Lista delova parcela
        /// </summary>
        public List<DeoParceleDto> DeloviParcele { get; set; }

    }
}
