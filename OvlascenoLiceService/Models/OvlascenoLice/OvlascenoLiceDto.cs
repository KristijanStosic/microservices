using OvlascenoLiceService.Models.BrojTable;
using System.Collections.Generic;

namespace OvlascenoLiceService.Models.OvlascenoLice
{
    /// <summary>
    /// Dto za ovlasceno lice
    /// </summary>
    public class OvlascenoLiceDto
    {
        /// <summary>
        /// Ime i prezime ovlascenog lica
        /// </summary>
        public string OvlascenoLice { get; set; }
        /// <summary>
        /// Licni dokument ovlascenog lica - jmbg za drzavljane Srbije, ili broj pasosa za strance
        /// </summary>
        public string BrojDokumenta { get; set; }
        /// <summary>
        /// Podaci o stanovanju - adresa za drzavljane Srbije, ili samo drzava za strance
        /// </summary>
        public string Stanovanje { get; set; }

        /// <summary>
        /// Lista brojeva tabli
        /// </summary>
        public List<BrojTableDto> BrojeviTabli { get; set; }
    }
}
