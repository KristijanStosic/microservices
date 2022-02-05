using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ParcelaService.Entities
{
    public class KatastarskaOpstina
    {
        [Key]
        public Guid KatastarskaOpstinaId { get; set; } = Guid.NewGuid();

        [Required]
        public string NazivKatastarskeOpstine { get; set; }
    }
}
