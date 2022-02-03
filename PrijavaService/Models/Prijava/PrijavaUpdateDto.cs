﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PrijavaService.Models.Prijava
{
    public class PrijavaUpdateDto
    {
        public Guid PrijavaId { get; set; }

        [Required(ErrorMessage = "Obavezno je uneti broj prijave.")]
        public string BrojPrijave { get; set; }

        [Required(ErrorMessage = "Obavezno je uneti datum prijave.")]
        public DateTime DatumPrijave { get; set; }
        public string MestoPrijave { get; set; }
        public string SatPrijema { get; set; }
        public bool ZatvorenaPonuda { get; set; }


        public List<Guid> JavnoNadmetanje { get; set; }
    }
}
