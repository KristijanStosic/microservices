using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using ZalbaService.Entities;
using ZalbaService.Models.Services;
using ZalbaService.Models.StatusZalbe;

namespace ZalbaService.Models.Zalba
{
    /// <summary>
    /// Dto za žalbu
    /// </summary>
    public class ZalbaDto
    {
        /// <summary>
        /// Id zalbe
        /// </summary>
        public Guid ZalbaId { get; set; }
        /// <summary>
        /// Datum podnošenja
        /// </summary>
        public DateTime DatumPodnosenja { get; set; }
        /// <summary>
        /// Datum rešenja
        /// </summary>
        public DateTime DatumResenja { get; set; }
        /// <summary>
        /// Razlog žalbe
        /// </summary>
        public string RazlogZalbe { get; set; }
        /// <summary>
        /// Obrazloženje
        /// </summary>
        public string Obrazlozenje { get; set; }
        /// <summary>
        /// Broj nadmetanja
        /// </summary>
        public string BrojNadmetanja { get; set; }
        /// <summary>
        /// Broj resenja
        /// </summary>
        public string BrojResenja { get; set; }
        /// <summary>
        /// Status zalbe
        /// </summary>
        public string StatusZalbe { get; set; }
        /// <summary>
        /// Tip zalbe
        /// </summary>
        public string TipZalbe { get; set; }
        /// <summary>
        /// Radnja za zalbu
        /// </summary>
        public string RadnjaZaZalbu { get; set; }
        /// <summary>
        /// Informacije o kupcu - Mikroservis Kupac
        /// </summary>
        public KupacDto Kupac { get; set; }
    }
}
