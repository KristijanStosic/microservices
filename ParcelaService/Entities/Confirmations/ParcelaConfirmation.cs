using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParcelaService.Entities.Confirmations
{
    public class ParcelaConfirmation
    {
        public Guid ParcelaId { get; set; } 
        public string BrojParcele { get; set; }
        public string PovrsinaParcele { get; set; }
        public string BrojListeNepokretnosti { get; set; }
    }
}
