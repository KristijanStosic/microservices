using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AdresaService.Model.Adresa
{
    /// <summary>
    /// Update Dto za adresu
    /// </summary>
    public class AdresaUpdateDto
    {
        /// <summary>
        /// Id adrese
        /// </summary>
        [Required]
        public Guid AdresaId { get; set; }
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
        /// Id države - strani ključ
        /// </summary>
        [Required]
        public Guid DrzavaId { get; set; }
     
    }
}
