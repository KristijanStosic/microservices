﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ParcelaService.Models.Klasa
{
    public class KlasaCreationDto
    {
        [Required(ErrorMessage ="Obavezno je uneti naziv klase!")]
        public string KlasaNaziv { get; set; }
    }
}
