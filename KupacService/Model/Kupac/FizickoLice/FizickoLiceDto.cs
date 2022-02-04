using KupacService.Model.OtherServices;
using KupacService.Model.Prioritet;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;


namespace KupacService.Model.Kupac.FizickoLice
{
    public class FizickoLiceDto
    {
        public Guid KupacId { get; set; }
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
        public List<PrioritetDto> Prioriteti { get; set; }
        public AdresaDto Adresa { get; set; }
       

    }
}
