using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ParcelaService.Models.Kultura
{
    public class KulturaCreationDto
    {
        [Required(ErrorMessage = "Obavezno je uneti naziv kulture!")]
        public string NazivKulture { get; set; }
    }
}
