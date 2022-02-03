using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZalbaService.Models
{
    /// <summary>
    /// Dto za radnju žalbe
    /// </summary>
    public class RadnjaZaZalbuDto
    {
        /// <summary>
        /// Id radnje za zalbu
        /// </summary>
        public Guid RadnjaZaZalbuId { get; set; }
        /// <summary>
        /// Naziv radnje žalbe
        /// </summary>
        public string NazivRadnjeZaZalbu { get; set; }
    }
}
