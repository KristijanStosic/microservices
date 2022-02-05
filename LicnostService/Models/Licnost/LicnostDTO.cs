﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LicnostService.Models.Licnost
{
    /// <summary>
    /// Model za ličnost
    /// </summary>
    public class LicnostDTO
    {
        /// <summary>
        /// ID ličnosti
        /// </summary>
        public Guid LicnostId { get; set; }

        /// <summary>
        /// Ime ličnosti
        /// </summary>
        public string Ime { get; set; }

        /// <summary>
        /// Prezime ličnosti
        /// </summary>
        public string Prezime { get; set; }

        /// <summary>
        /// Funkcija ličnosti
        /// </summary>
        public string Funkcija { get; set; }



    }
}
