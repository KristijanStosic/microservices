using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ParcelaService.Models.ZasticenaZona
{
    public class ZasticenaZonaUpdateDto
    {
        public Guid ZasticenaZonaId { get; set; }
        [Required(ErrorMessage ="Obavezno je uneti broj zasticene zone!")]
        public string BrojZasticeneZone { get; set; }
    }
}
