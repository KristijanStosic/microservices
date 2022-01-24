﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AdresaService.Entities
{
    public class Adresa
    {
        [Key]
        public Guid AdresaId { get; set; } = Guid.NewGuid();
        public string Ulica { get; set; }
        public string Broj { get; set; }
        public string Mesto { get; set; }
        public string PostanskiBroj { get; set; }
        
        public Guid DrzavaId { get; set; }
        public Drzava Drzava { get; set; }



    }
}
