using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KupacService.Model.Kupac.FizickoLice
{
    public class FizickoLiceConfirmDto
    {
        public string Ime { get; set; }
        public string Prezime { get; set; }
        public string JMBG { get; set; }
        public string BrojTelefona { get; set; }
        public string Email { get; set; }
        public string BrojRacuna { get; set; }
    }
}
