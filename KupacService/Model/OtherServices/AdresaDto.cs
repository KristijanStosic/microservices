using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KupacService.Model.OtherServices
{
    /// <summary>
    /// Dto za adresu
    /// </summary>
    public class AdresaDto
    {
        /// <summary>
        /// Naziv ulice
        /// </summary>
        public string Ulica { get; set; }
        /// <summary>
        /// Kucni broj
        /// </summary>
        public string Broj { get; set; }
        /// <summary>
        /// Mesto stanovanja ovlascenog lica
        /// </summary>
        public string Mesto { get; set; }
        /// <summary>
        /// Postanski broj mesta
        /// </summary>
        public string PostanskiBroj { get; set; }
        /// <summary>
        /// Naziv Drzave adrese
        /// </summary>
        public string Drzava { get; set; }
    }
}
