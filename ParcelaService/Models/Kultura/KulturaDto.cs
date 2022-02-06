using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParcelaService.Models.Kultura
{
    /// <summary>
    /// Model za kulturu parcele
    /// </summary>
    public class KulturaDto
    {
        /// <summary>
        /// Id kulture parcele
        /// </summary>
        public Guid KulturaId { get; set; }
        /// <summary>
        /// Naziv kulture
        /// </summary>
        public string NazivKulture { get; set; }
    }
}
