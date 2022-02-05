using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KupacService.Entities.ManyToMany
{
    public class KupacUplata
    {
        public Guid KupacId { get; set; }
        public Kupac Kupac { get; set; }
        public Guid UplataId { get; set; }
    }
}
