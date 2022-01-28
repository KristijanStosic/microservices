using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KupacService.Entities
{
    public class Prioritet
    {

        [Key]
        public Guid PrioritetId { get; set; } = Guid.NewGuid();
        public string Opis { get; set; }
        public virtual List<Kupac> Kupci { get; set; }



    }
}
