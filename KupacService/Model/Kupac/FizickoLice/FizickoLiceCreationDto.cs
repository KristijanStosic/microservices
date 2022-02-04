using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KupacService.Model.Kupac.FizickoLice
{
    public class FizickoLiceCreationDto
    {
        public string Ime { get; set; }
        public string Prezime { get; set; }
        public string JMBG { get; set; }
        public double OstvarenaPovrsina { get; set; }
        public bool ImaZabranu { get; set; }
        [DataType(DataType.Date)]
        public DateTime DatumPocetkaZabrane { get; set; }
        public int DuzinaTrajanjaZabraneGod { get; set; }
        public string BrojTelefona { get; set; }
        public string BrojTelefona2 { get; set; }
        public string Email { get; set; }
        public string BrojRacuna { get; set; }
        public List<Guid> Prioriteti { get; set; }
        public Guid AdresaId { get; set; }
        public List<Guid> OvlascenaLica { get; set; }
        public List<Guid> Uplate { get; set; }
    }
}
