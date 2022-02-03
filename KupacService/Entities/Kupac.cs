using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KupacService.Entities
{
    public abstract class Kupac
    {
        [Key]
        public Guid KupacId { get; set; } = Guid.NewGuid();
        public double OstvarenaPovrsina { get; set; }
        public bool ImaZabranu { get; set; }
        [DataType(DataType.Date)]
        public DateTime DatumPocetkaZabrane { get; set; }
        public int DuzinaTrajanjaZabraneGod { get; set; }
        public string BrojTelefona { get; set; }
        public string BrojTelefona2 { get; set; }
        public string Email { get; set; }
        public string BrojRacuna { get; set; }

        public virtual List<Prioritet> Prioriteti { get; set; }


        public Guid? AdresaId { get; set; }

    }
}
