using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KupacService.Entities.ManyToMany
{
    public class KupacOvlascenoLice
    {
        public Guid KupacId { get; set; }
        public Kupac Kupac { get; set; }
        public Guid OvlascenoLiceId { get; set; }
    }
}
