using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParcelaService.Models.Obradivost
{
    /// <summary>
    /// Model za obradivost parcele
    /// </summary>
    public class ObradivostDto
    {
        /// <summary>
        /// Id obradivosti parele
        /// </summary>
        public Guid ObradivostId { get; set; }
        /// <summary>
        /// Opis obradivosti parcele
        /// </summary>
        public string OpisObradivosti { get; set; }
    }
}
