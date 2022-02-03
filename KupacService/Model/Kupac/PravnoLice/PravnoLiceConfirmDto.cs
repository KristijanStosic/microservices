using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KupacService.Model.Kupac.PravnoLice
{
    public class PravnoLiceConfirmDto
    {
        public string Naziv { get; set; }
        public string MaticniBroj { get; set; }
        public string BrojTelefona { get; set; }
        public string Email { get; set; }
        public string BrojRacuna { get; set; }
    }
}
