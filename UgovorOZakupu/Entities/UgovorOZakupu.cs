using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace UgovorOZakupu.Entities
{
    [Table("UgovorOZakupu")]
    public class UgovorOZakupu
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string ZavodniBroj { get; set; }

        public DateTime DatumZavodjenja { get; set; }

        public DateTime RokZaVracanje { get; set; }

        public string MestoPotpisivanja { get; set; }

        public DateTime DatumPotpisivanja { get; set; }

        public Guid TipGarancijeId { get; set; }
        public TipGarancije TipGarancije { get; set; }

        public IEnumerable<RokDospeca> RokoviDospeca { get; set; }

        public Guid DokumentId { get; set; }
        
        public Guid JavnoNadmetanjeId { get; set; }
    }
}