using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZalbaService.Models
{
    /// <summary>
    /// Dto za tip žalbe
    /// </summary>
    public class TipZalbeDto
    {
        /// <summary>
        /// Id tipa zalbe
        /// </summary>
        public Guid TipZalbeId { get; set; }
        /// <summary>
        /// Naziv tipa žalbe
        /// </summary>
        public string NazivTipaZalbe { get; set; }
    }
}
