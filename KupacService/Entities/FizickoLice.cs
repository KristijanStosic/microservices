using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace KupacService.Entities
{
    [Table("FizickoLice")]
    public class FizickoLice: Kupac
    {

        public string Ime { get; set; }
        public string Prezime { get; set; }
        public string JMBG { get; set; }
    }
}
