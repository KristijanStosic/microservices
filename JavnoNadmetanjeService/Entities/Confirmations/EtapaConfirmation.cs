using System;

namespace JavnoNadmetanjeService.Entities.Confirmations
{
    public class EtapaConfirmation
    {
        public Guid EtapaId { get; set; }
        public DateTime Datum { get; set; }
        public int DanPoRedu { get; set; }
        public TimeSpan VremePocetka { get; set; }
    }
}
