using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParcelaService.Entities.Confirmations
{
    public class DeoParceleConfirmation
    {
        public Guid DeoParceleId { get; set; } 
        public string RedniBrojDela { get; set; }
        public string PovrsinaDela { get; set; }
    }
}
