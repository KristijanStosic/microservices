using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ZalbaService.Entities
{
    /// <summary>
    /// Predstavlja Žalbu
    /// </summary>
    public class Zalba
    {
        /// <summary>
        /// ID žalbe
        /// </summary>
        [Key]
        public Guid ZalbaId { get; set; } = Guid.NewGuid();

        /// <summary>
        /// Datum podnosenja
        /// </summary>
        [Required]
        public DateTime DatumPodnosenja { get; set; }

        /// <summary>
        /// Datum resenja
        /// </summary>
        [Required]
        public DateTime DatumResenja { get; set; }

        /// <summary>
        /// Razlog zalbe
        /// </summary>
        [Required]
        public string RazlogZalbe { get; set; }

        /// <summary>
        /// Obrazlozenje
        /// </summary>
        [Required]
        public string Obrazlozenje { get; set; }

        /// <summary>
        /// Broj nadmetanja
        /// </summary>
        [Required]
        public string BrojNadmetanja { get; set; }

        /// <summary>
        /// Broj resenja
        /// </summary>
        [Required]
        public string BrojResenja { get; set; }

        /// <summary>
        /// ID statusa žalbe - strani kljuc
        /// </summary>
        public Guid StatusZalbeId { get; set; }

        /// <summary>
        /// Objekat statusa žalbe
        /// </summary>
        public StatusZalbe StatusZalbe { get; set; }
        /// <summary>
        /// ID tipa žalbe - strani kljuc
        /// </summary>
        public Guid TipZalbeId { get; set; }
        /// <summary>
        /// Objekat tipa žalbe
        /// </summary>
        public TipZalbe TipZalbe { get; set; }
        /// <summary>
        /// ID radnje za žalbu - strani kljuc
        /// </summary>
        public Guid RadnjaZaZalbuId { get; set; }
        /// <summary>
        /// Objekat radnje za žalbu
        /// </summary>
        public RadnjaZaZalbu RadnjaZaZalbu { get; set; }

        /// <summary>
        /// Id kupca - veza sa mikroservisom Kupac 
        /// </summary>
        public Guid? KupacId { get; set; }
    }
}
