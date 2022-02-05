using System;
using System.Collections.Generic;

namespace UgovorOZakupu.Models.JavnoNadmetanje
{
    /// <summary>
    /// Model za ovlašćeno lice
    /// </summary>
    public class OvlascenoLiceDto
    {
        /// <summary>
        /// ID ovlašćenog lica
        /// </summary>
        public Guid OvlascenoLiceId { get; set; }
        
        /// <summary>
        /// Ime i prezime ovlašćenog lica
        /// </summary>
        public string OvlascenoLice { get; set; }
        
        /// <summary>
        /// Lični dokument ovlašćenog lica - jmbg za državljane Srbije, ili broj pasoša za strance
        /// </summary>
        public string BrojDokumenta { get; set; }
        
        /// <summary>
        /// Podaci o stanovanju - adresa za državljane Srbije, ili samo država za strance
        /// </summary>
        public string Stanovanje { get; set; }

        /// <summary>
        /// Lista brojeva tabli
        /// </summary>
        public List<BrojTableDto> BrojeviTabli { get; set; }
    }
}