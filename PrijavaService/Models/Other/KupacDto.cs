using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrijavaService.Models.Other
{
    public class KupacDto
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
