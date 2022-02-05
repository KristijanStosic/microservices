using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdresaService.Model.Drzava
{
    /// <summary>
    /// Dto za državu
    /// </summary>
    public class DrzavaDto
    {
        /// <summary>
        /// ID države
        /// </summary>
        public Guid DrzavaId { get; set; }
        /// <summary>
        /// Naziv Države
        /// </summary>
        public string NazivDrzave { get; set; }
    }
}
