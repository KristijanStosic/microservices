﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ZalbaService.Models
{
    public class TipZalbeCreateDto 
    {
        [Required(ErrorMessage = "Obavezno je uneti naziv tipa zalbe")]
        public string NazivTipaZalbe { get; set; }
    }
}
