using System;

namespace PrijavaService.Entities.Confirmations
{
    public class PrijavaConfirmation
    {
        public Guid PrijavaId { get; set; }
        public string BrojPrijave { get; set; }
        public DateTime DatumPrijave { get; set; }
        public string MestoPrijave { get; set; }
    }
}
