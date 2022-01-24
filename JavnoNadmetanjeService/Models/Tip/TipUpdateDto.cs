﻿using System;
using System.ComponentModel.DataAnnotations;

namespace JavnoNadmetanjeService.Models.Tip
{
    public class TipUpdateDto
    {
        public Guid TipId { get; set; }
        [Required(ErrorMessage = "Obavezno je uneti naziv tipa javnog nadmetanja")]
        public string NazivTipa { get; set; }
    }
}