using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ParcelaService.Entities
{
    public class Klasa
    {
        [Key]
        public Guid ID_Klasa { get; set; } = Guid.NewGuid();

        [Required]
        public string KlasaNaziv { get; set; }
    }
}
