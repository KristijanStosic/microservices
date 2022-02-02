﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ParcelaService.Models.KatastarskaOpstina
{
    public class KatastarskaOpstinaCreationDto
    {
        [Required(ErrorMessage ="Obavezno je uneti naziv katastarske opstine")]
        public string NazivKatastarskeOpstine { get; set; }
    }
}
