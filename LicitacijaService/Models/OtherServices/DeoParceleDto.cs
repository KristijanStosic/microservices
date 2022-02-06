using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LicitacijaService.Models.OtherServices
{
    public class DeoParceleDto
    {
        public string BrojParcele { get; set; }
        public int RbDela { get; set; }
        public int PovrsinaDela { get; set; }
        public string Kultura { get; set; }
        public string Klasa { get; set; }
        public string Obradivost { get; set; }
        public string ZasticenaZona { get; set; }
        public string Odvodnjavanje { get; set; }
        public string KatastarskaOpstina { get; set; }
    }
}
