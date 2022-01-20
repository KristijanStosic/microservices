using System;

namespace JavnoNadmetanjeService.Entities.Confirmations
{
    public class JavnoNadmetanjeConfirmation
    {
        public Guid JavnoNadmetanjeId { get; set; }
        public double PocetnaCenaHektar { get; set; }
        public int PeriodZakupa { get; set; }
        public int Krug { get; set; }
        public int VisinaDopuneDepozita { get; set; }
    }
}
