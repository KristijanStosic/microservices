using System;
using System.Collections.Generic;
using UgovorOZakupu.Models.RokDospeca;
using UgovorOZakupu.Models.TipGarancije;

namespace UgovorOZakupu.Models.UgovorOZakupu
{
    public class UgovorOZakupuDto
    {
        public string ZavodniBroj { get; set; }

        public DateTime DatumZavodjenja { get; set; }

        public DateTime RokZaVracanje { get; set; }

        public string MestoPotpisivanja { get; set; }

        public DateTime DatumPotpisivanja { get; set; }

        public TipGarancijeDto TipGarancije { get; set; }

        public IEnumerable<RokDospecaDto> RokoviDospeca { get; set; }
    }
}