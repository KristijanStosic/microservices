using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ParcelaService.Models.KatastarskaOpstina
{
    public class KatastarskaOpstinaUpdateDto
    {
        public Guid KatastarskaOpstinaId { get; set; }

        [Required(ErrorMessage ="Obavezno je uneti naziv katastarske opstine")]
        public string NazivKatastarskeOpstine { get; set; }
    }
}
