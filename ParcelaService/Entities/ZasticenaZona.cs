﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ParcelaService.Entities
{
    public class ZasticenaZona
    {
        [Key]
        public Guid ZasticenaZonaId { get; set; } = Guid.NewGuid();
        [Required]
        public string BrojZasticeneZone { get; set; }
    }
}
