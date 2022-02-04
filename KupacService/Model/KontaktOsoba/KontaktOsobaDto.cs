using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KupacService.Model.KontaktOsoba
{
    public class KontaktOsobaDto
    {
        public Guid KontaktOsobaId { get; set; }
        public string Ime { get; set; }
        public string Prezime { get; set; }
        public string Telefon { get; set; }
        public string Funkcija { get; set; }
    }
}
