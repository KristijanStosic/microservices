using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrijavaService.Entities.Confirmations
{
    public class DokPravnaLicaConfirmation
    {
        public Guid DokPravnaLicaId { get; set; }
        public string NazivDokumenta { get; set; }
    }
}
