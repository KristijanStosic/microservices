using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParcelaService.Models.KatastarskaOpstina
{
    /// <summary>
    /// Model za katastarsku opstinu parcele
    /// </summary>
    public class KatastarskaOpstinaDto
    {
        /// <summary>
        /// Id katastarske opstine
        /// </summary>

        public Guid KatastarskaOpstinaId { get; set; }

        /// <summary>
        /// Naziv katastarske opstine
        /// </summary>
        public string NazivKatastarskeOpstine { get; set; }
    }
}
