using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KupacService.Model.Kupac
{
    /// <summary>
    /// Kupac info za pristup od strane drugih servisa
    /// </summary>
    public class KupacInfoDto
    {
        /// <summary>
        /// Id kupca
        /// </summary>
        public Guid KupacId { get; set; }
        /// <summary>
        /// Naziv kupca
        /// </summary>
        public string Naziv { get; set; }
        /// <summary>
        /// Broj telefona kupca
        /// </summary>
        public string BrojTelefona { get; set; }
        /// <summary>
        /// Emali kupca
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// Broj računa kupca
        /// </summary>
        public string BrojRacuna { get; set; }
    }
}
