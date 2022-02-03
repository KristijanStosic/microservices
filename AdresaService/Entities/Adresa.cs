using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AdresaService.Entities
{
    /// <summary>
    /// Predstavlja adresu
    /// </summary>
    public class Adresa
    {
        /// <summary>
        /// Id adrese
        /// </summary>
        [Key]
        public Guid AdresaId { get; set; } = Guid.NewGuid();
        /// <summary>
        /// Naziv ulice
        /// </summary>
        public string Ulica { get; set; }
        /// <summary>
        /// Broj adrese 
        /// </summary>
        public string Broj { get; set; }
        /// <summary>
        /// Mesto kom adresa pripada
        /// </summary>
        public string Mesto { get; set; }
        /// <summary>
        /// Poštanski broj
        /// </summary>
        public string PostanskiBroj { get; set; }
        /// <summary>
        /// id države - strani ključ
        /// </summary>
        public Guid DrzavaId { get; set; }
        /// <summary>
        /// Objekat država
        /// </summary>
        public Drzava Drzava { get; set; }



    }
}
