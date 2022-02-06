using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LicitacijaService.Models
{
    /// <summary>
    /// Model za izmenu licitacije
    /// </summary>
    public class LicitacijaUpdateDto
    {
        /// <summary>
        /// Id licitacije
        /// </summary>
        public Guid LicitacijaId { get; set; }
        /// <summary>
        /// Broj licitacije
        /// </summary>
        [Required(ErrorMessage = "Obavezno je uneti broj licitacije")]
        public int BrojLicitacije { get; set; }
        /// <summary>
        /// Godina licitacije
        /// </summary>
        [Required(ErrorMessage = "Obavezno je uneti godinu licitacije")]
        public int GodinaLicitacije { get; set; }
        /// <summary>
        /// Ogranicenje licitacije
        /// </summary>
        public int OgranicenjeLicitacije { get; set; }
        /// <summary>
        /// Rok licitacije
        /// </summary>
        public DateTime RokLicitacije { get; set; }
        /// <summary>
        /// Korak cene
        /// </summary>
        [Required(ErrorMessage = "Obavezno je uneti korak cene licitacije")]
        public int KorakCeneLicitacije { get; set; }

        /// <summary>
        /// Lista id-eva javnih nadmetanja
        /// </summary>
        public List<Guid> JavnaNadmetanja { get; set; }

    }
}
