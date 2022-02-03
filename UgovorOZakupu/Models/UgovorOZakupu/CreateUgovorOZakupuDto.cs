using System;
using System.Collections.Generic;

namespace UgovorOZakupu.Models.UgovorOZakupu
{
    public class CreateUgovorOZakupuDto
    {
        public string ZavodniBroj { get; set; }

        public DateTime DatumZavodjenja { get; set; }

        public DateTime RokZaVracanje { get; set; }

        public string MestoPotpisivanja { get; set; }

        public DateTime DatumPotpisivanja { get; set; }

        public Guid TipGarancijeId { get; set; }

        public IEnumerable<int> RokoviDospeca { get; set; }

        public Guid DokumentId { get; set; }
    }
}