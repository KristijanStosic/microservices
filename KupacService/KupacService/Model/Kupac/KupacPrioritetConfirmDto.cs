using KupacService.Model.Prioritet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KupacService.Model.Kupac
{
    public class KupacPrioritetConfirmDto
    {

        public string Naziv { get; set; }
        public List<PrioritetDto> Prioriteti { get; set; }



    }
}
