using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace KupacService.Entities
{
    [Table("PravnoLice")]
    public class PravnoLice : Kupac
    {
   
        public string Naziv { get; set; }
        public string MaticniBroj { get; set; }
        public string Faks { get; set; }
        public Guid KontaktOsobaId { get; set; }
        public KontaktOsoba KontaktOsoba { get; set; }
    }
}
