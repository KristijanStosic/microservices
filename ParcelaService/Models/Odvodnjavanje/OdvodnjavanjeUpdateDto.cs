using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ParcelaService.Models.Odvodnjavanje
{
    public class OdvodnjavanjeUpdateDto
    {
        public Guid OdvodnjavanjeId { get; set; }
        [Required(ErrorMessage ="Obavezno je uneti opis odvodnjavanja!")]
        public string OpisOdvodnjavanja { get; set; }
    }
}
