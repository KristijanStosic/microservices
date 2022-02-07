using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KupacService.Model.Kupac.PravnoLice
{
    public class PravnoLiceCreateDto
    {
        public string Naziv { get; set; }
        public string MaticniBroj { get; set; }
        public string Faks { get; set; }
        public double OstvarenaPovrsina { get; set; }
        public bool ImaZabranu { get; set; }
        [DataType(DataType.Date)]
        public DateTime DatumPocetkaZabrane { get; set; }
        public int DuzinaTrajanjaZabraneGod { get; set; }
        public string BrojTelefona { get; set; }
        public string BrojTelefona2 { get; set; }
        public string Email { get; set; }
        public string BrojRacuna { get; set; }
        [Required]
        public Guid KontaktOsobaId { get; set; }
        public List<Guid> Prioriteti { get; set; }
        public Guid AdresaId { get; set; }
        public List<Guid> OvlascenaLica { get; set; }
        public List<Guid> Uplate { get; set; }
    }
}
