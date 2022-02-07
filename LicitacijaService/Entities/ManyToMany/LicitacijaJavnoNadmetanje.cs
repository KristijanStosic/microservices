using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LicitacijaService.Entities.ManyToMany
{
    public class LicitacijaJavnoNadmetanje
    {
        public Guid LicitacijaId { get; set; }
        public Licitacija Licitacija { get; set; }

        public Guid JavnoNadmetanjeId { get; set; }
    }
}
