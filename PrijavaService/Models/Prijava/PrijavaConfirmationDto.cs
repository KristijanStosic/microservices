using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrijavaService.Models.Prijava
{
    public class PrijavaConfirmationDto
    {
        public string BrojPrijave { get; set; }
        public DateTime DatumPrijave { get; set; }
        public string MestoPrijave { get; set; }
    }
}
