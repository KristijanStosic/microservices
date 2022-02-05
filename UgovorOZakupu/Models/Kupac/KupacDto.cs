using System;
using System.Collections.Generic;
using UgovorOZakupu.Models.JavnoNadmetanje;

namespace UgovorOZakupu.Models.Kupac
{
    public class KupacDto
    {
        public Guid KupacId { get; set; }
        public string Naziv { get; set; }
        public string Faks { get; set; }
        public double OstvarenaPovrsina { get; set; }
        public bool ImaZabranu { get; set; }
        public DateTime DatumPocetkaZabrane { get; set; }
        public int DuzinaTrajanjaZabraneGod { get; set; }
        public string BrojTelefona { get; set; }
        public string BrojTelefona2 { get; set; }
        public string Email { get; set; }
        public string BrojRacuna { get; set; }
        public KontaktOsobaDto KontaktOsoba { get; set; }

        public List<PrioritetDto> Prioriteti { get; set; }
        // public AdresaDto Adresa { get; set; }
        public List<OvlascenoLiceDto> OvlascenaLica { get; set; }
    }
}