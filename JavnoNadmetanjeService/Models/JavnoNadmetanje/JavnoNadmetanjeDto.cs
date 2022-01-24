using System.Collections.Generic;
using JavnoNadmetanjeService.Models.Tip;
using JavnoNadmetanjeService.Models.Status;
using JavnoNadmetanjeService.Models.Etapa;
using JavnoNadmetanjeService.Models.Other;

namespace JavnoNadmetanjeService.Models.JavnoNadmetanje
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
        public List<EtapaDto> Etape { get; set; }

        public string Adresa { get; set; }
        public string Kupac { get; set; }
    }
}
