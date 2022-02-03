using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ParcelaService.Models.KatastarskaOpstina
{
    /// <summary>
    /// Model za izmenu katastarske opstine 
    /// </summary>
    public class KatastarskaOpstinaUpdateDto
    {
        /// <summary>
        /// Id katastarske opstine
        /// </summary>
        
        public Guid KatastarskaOpstinaId { get; set; }

        /// <summary>
        /// Naziv katastarske opstine
        /// </summary>
        [Required(ErrorMessage ="Obavezno je uneti naziv katastarske opstine")]
        public string NazivKatastarskeOpstine { get; set; }
    }
}
