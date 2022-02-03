using PrijavaService.Entities;
using PrijavaService.Models.Other;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrijavaService.Models.Prijava
{
    public class PrijavaDto
    {
        public Guid PrijavaId { get; set; }
        public string BrojPrijave { get; set; }
        public DateTime DatumPrijave { get; set; }
        public string MestoPrijave { get; set; }
        public string SatPrijema { get; set; }
        public bool ZatvorenaPonuda { get; set; }
        public List<DokFizickaLica> DokFizickaLica { get; set; }
        public List<DokPravnaLica> DokPravnaLica { get; set; }

        public List<JavnoNadmetanjeDto> JavnoNadmetanje { get; set; }

    }
}
