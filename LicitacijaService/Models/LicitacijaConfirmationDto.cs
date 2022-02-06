using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LicitacijaService.Models
{
    /// <summary>
    /// Model za potvrdu kreiranja licitacije
    /// </summary>
    public class LicitacijaConfirmationDto
    {
        /// <summary>
        /// Broj licitacije
        /// </summary>
        public int BrojLicitacije { get; set; }
        /// <summary>
        /// Godina licitacije
        /// </summary>
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
        public int KorakCeneLicitacije { get; set; }
    }
}
