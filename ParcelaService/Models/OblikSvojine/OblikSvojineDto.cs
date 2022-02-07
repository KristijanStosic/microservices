using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParcelaService.Models.OblikSvojine
{
    /// <summary>
    /// Model za oblik svojine parcele
    /// </summary>
    public class OblikSvojineDto
    {
        /// <summary>
        /// Id oblika svojine
        /// </summary>
        public Guid OblikSvojineId { get; set; }

        /// <summary>
        /// Opis oblika svojine
        /// </summary>
        public string OpisOblikaSvojine { get; set; }
    }
}
