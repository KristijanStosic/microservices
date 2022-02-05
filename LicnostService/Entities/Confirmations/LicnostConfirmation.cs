using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LicnostService.Entities.Confirmations
{
    public class LicnostConfirmation
    {

        public Guid LicnostId { get; set; }

        public string Ime { get; set; }
        
        public string Prezime { get; set; }


    }
}
