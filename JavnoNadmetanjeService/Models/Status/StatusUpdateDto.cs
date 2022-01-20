﻿using System;
using System.ComponentModel.DataAnnotations;

namespace JavnoNadmetanjeService.Models.Status
{
    public class StatusUpdateDto
    {
        public Guid StatusId { get; set; }
        [Required(ErrorMessage = "Obavezno je uneti naziv statusa javnog nadmetanja")]
        public string NazivStatusa { get; set; }
    }
}
