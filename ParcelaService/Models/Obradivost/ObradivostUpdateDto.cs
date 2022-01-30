﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ParcelaService.Models.Obradivost
{
    public class ObradivostUpdateDto
    {
        public Guid ObradivostId { get; set; }
        [Required(ErrorMessage ="Obavezno je uneti opis obradivosti!")]
        public string OpisObradivosti { get; set; }

    }
}
