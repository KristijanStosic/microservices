using System;

namespace UgovorOZakupu.Models.UgovorOZakupu
{
    public class UpdateUgovorOZakupuDto
    {
        public Guid Id { get; set; }

        public string ZavodniBroj { get; set; }

        public DateTime DatumZavodjenja { get; set; }

        public DateTime RokZaVracanje { get; set; }

        public string MestoPotpisivanja { get; set; }

        public DateTime DatumPotpisivanja { get; set; }

        public Guid TipGarancijeId { get; set; }
        
        public Guid DokumentId { get; set; } // Odluka
        
        public Guid JavnoNadmetanjeId { get; set; }
        
        public Guid KupacId { get; set; } // Lice
        
        public Guid LicnostId { get; set; } // Ministar
    }
}