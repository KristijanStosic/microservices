using System;

namespace JavnoNadmetanjeService.Models.JavnoNadmetanje
{
    public class JavnoNadmetanjeCreationDto
    {
        public double PocetnaCenaHektar { get; set; }
        public int VisinaDopuneDepozita { get; set; }
        public int PeriodZakupa { get; set; }
        public int IzlicitiranaCena { get; set; }
        public int BrojUcesnika { get; set; }
        public int Krug { get; set; }
        public bool Izuzeto { get; set; }
        public Guid StatusId { get; set; }
        public Guid TipId { get; set; }
    }
}
