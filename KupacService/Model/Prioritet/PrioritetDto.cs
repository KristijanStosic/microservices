using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KupacService.Model.Prioritet
{   /// <summary>
/// Dto za prioritet
/// </summary>
    public class PrioritetDto
    {
        /// <summary>
        /// Id prioriteta
        /// </summary>
        public Guid PrioritetId { get; set; }
        /// <summary>
        /// Opis prioriteta
        /// </summary>
        public string Opis { get; set; }
    }
}
