using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdresaService.Model.Adresa
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
        /// Broj adrese
        /// </summary>
        public string Broj { get; set; }
        /// <summary>
        /// Mesto u kom se adresa nalazi
        /// </summary>
        public string Mesto { get; set; }
        /// <summary>
        /// Poštanski broj
        /// </summary>
        public string PostanskiBroj { get; set; }
        /// <summary>
        /// Naziv države
        /// </summary>
        public string Drzava { get; set; }
    }
}
