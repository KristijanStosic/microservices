using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LicnostService.Models.Licnost;

namespace LicnostService.Entities.Confirmations
{
    public class KomisijaConfirmation
    {
        public Guid KomisijaId { get; set; }
        public string NazivKomisije { get; set; }

    }
}
