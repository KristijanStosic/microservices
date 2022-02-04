using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdresaService.Model.Drzava
{
    /// <summary>
    /// Update Dto za državu
    /// </summary>
    public class DrzavaUpdateDto
    {
        /// <summary>
        /// Id države
        /// </summary>
        public Guid DrzavaId { get; set; }
        /// <summary>
        /// Naziv države
        /// </summary>
        public string NazivDrzave { get; set; }
    }
}
