using System;

namespace JavnoNadmetanjeService.Models.Etapa
{
    public class EtapaDto
    {
        public DateTime Datum { get; set; }
        public int DanPoRedu { get; set; }
        public string VremePocetka { get; set; }
        public string VremeKraja { get; set; }
        public bool ZavrsenaUspesno { get; set; }
    }
}
