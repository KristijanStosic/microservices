using System;
using System.Collections.Generic;
using UgovorOZakupu.Models.Dokument;
using UgovorOZakupu.Models.JavnoNadmetanje;

namespace UgovorOZakupu.Models.UgovorOZakupu
{
    public class UgovorOZakupuDto
    {
        public string ZavodniBroj { get; set; }

        public DateTime DatumZavodjenja { get; set; }

        public DateTime RokZaVracanje { get; set; }

        public string MestoPotpisivanja { get; set; }

        public DateTime DatumPotpisivanja { get; set; }

        public string TipGarancije { get; set; }

        public IEnumerable<int> RokoviDospeca { get; set; }

        public DokumentDto Dokument { get; set; }
        
        public JavnoNadmetanjeDto JavnoNadmetanje { get; set; }
    }
}