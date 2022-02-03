using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrijavaService.Models.Other
{
    public class JavnoNadmetanjeDto
    {
        public double PocetnaCenaHektar { get; set; }
        public int VisinaDopuneDepozita { get; set; }
        public int PeriodZakupa { get; set; }
        public int IzlicitiranaCena { get; set; }
        public int BrojUcesnika { get; set; }
        public int Krug { get; set; }
        public bool Izuzeto { get; set; }
        public string Status { get; set; }
        public string Tip { get; set; }
    }
}
